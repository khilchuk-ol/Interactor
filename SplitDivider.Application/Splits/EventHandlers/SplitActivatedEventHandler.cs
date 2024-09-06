using System.Diagnostics;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Application.Splits.Graph.Interfaces;
using SplitDivider.Domain.Enums;
using SplitDivider.Domain.Events;

namespace SplitDivider.Application.Splits.EventHandlers;

public class SplitActivatedEventHandler : INotificationHandler<SplitActivatedEvent>
{
    private readonly ILogger<SplitActivatedEventHandler> _logger;

    private readonly IApplicationDbContext _context;

    private readonly IGraphBuilder _graphBuilder;
    
    private readonly IGraphPartitioner _graphPartitioner;

    private readonly IPerformanceTracker _perfTracker;

    private const string FETCH_USERS_OPERATION = "fetch users from database";
    private const string BUILD_GRAPH_OPERATION = "build graph";
    private const string CUT_GRAPH_OPERATION = "cut graph";
    private const string SAVE_GROUPS_OPERATION = "save data to database";

    public SplitActivatedEventHandler(
        ILogger<SplitActivatedEventHandler> logger,
        IApplicationDbContext context,
        IGraphBuilder graphBuilder,
        IGraphPartitioner graphPartitioner,
        IPerformanceTracker perfTracker
    )
    {
        _logger = logger;
        _context = context;
        _graphBuilder = graphBuilder;
        _graphPartitioner = graphPartitioner;
        _perfTracker = perfTracker;
    }
    
    public async Task Handle(SplitActivatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Split activated: Id={Id}", notification.Split.Id);

        var operations = new Dictionary<string, long>();

        var generalSw = Stopwatch.StartNew();
        var indSw = Stopwatch.StartNew();

        var split = notification.Split;
        
        var usersInSplitQuery = _context.AppUsers.AsQueryable();

        if (split.Gender != null)
        {
            usersInSplitQuery = usersInSplitQuery.Where(u => u.Gender == split.Gender);
        }
        
        if (split.MinRegistrationDt != null)
        {
            usersInSplitQuery = usersInSplitQuery.Where(u => u.RegistrationDt >= split.MinRegistrationDt);
        }
        
        if (split.CountryIds is { Count: > 0 })
        {
            usersInSplitQuery = usersInSplitQuery.Where(u => split.CountryIds.Contains(u.CountryId));
        }

        var usersInSplit = await usersInSplitQuery
            .AsNoTracking()
            .Select(u => u.Id)
            .ToListAsync(cancellationToken);

        operations[FETCH_USERS_OPERATION] = indSw.ElapsedMilliseconds;
        indSw.Stop();

        indSw = Stopwatch.StartNew();

        var graphDto = _graphBuilder.BuildGraph(split, usersInSplit); // async ?
        
        operations[BUILD_GRAPH_OPERATION] = indSw.ElapsedMilliseconds;
        indSw.Stop();
        
        indSw = Stopwatch.StartNew();

        var groups = _graphPartitioner.PartitionSplitGraph(graphDto);
        
        operations[CUT_GRAPH_OPERATION] = indSw.ElapsedMilliseconds;
        indSw.Stop();

        indSw = Stopwatch.StartNew();
        
        foreach (var id in groups.first)
        {
            var userGroup = new UserSplit
            {
                UserId = id,
                SplitId = split.Id,
                Group = UserSplit.GroupControl
            };

            _context.UserSplits.Add(userGroup);
        }
        
        foreach (var id in groups.second)
        {
            var userGroup = new UserSplit
            {
                UserId = id,
                SplitId = split.Id,
                Group = UserSplit.GroupTest
            };

            _context.UserSplits.Add(userGroup);
        }

        var splitEntity = await _context.Splits.FindAsync(split.Id);
        splitEntity!.State = SplitState.ReadyToTest;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        operations[SAVE_GROUPS_OPERATION] = indSw.ElapsedMilliseconds;
        indSw.Stop();
        
        generalSw.Stop();
        
        _perfTracker.TrackPerformance($"Split{split.Id} {_graphPartitioner.GetName()} (opt. db, parallel impr.) graph cut (vertices: {graphDto.Graph.VerticesCount})", generalSw.ElapsedMilliseconds, operations.Select(p => $"{p.Key} in {p.Value}ms").ToList());
    }
}
