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
    
    private readonly IGraphCutter _graphCutter;

    public SplitActivatedEventHandler(
        ILogger<SplitActivatedEventHandler> logger,
        IApplicationDbContext context,
        IGraphBuilder graphBuilder,
        IGraphCutter graphCutter
    )
    {
        _logger = logger;
        _context = context;
        _graphBuilder = graphBuilder;
        _graphCutter = graphCutter;
    }
    
    public async Task Handle(SplitActivatedEvent notification, CancellationToken cancellationToken)
    {
        if (notification == null) throw new ArgumentNullException(nameof(notification));
        
        _logger.LogInformation("Split activated: Id={Id}", notification.Split.Id);

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
            .Select(u => u.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(true);

        var graphDto = _graphBuilder.BuildGraph(split, usersInSplit);

        var groups = _graphCutter.CutSplitGraph(graphDto);

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

        var splitEntity = await _context.Splits.FindAsync(split.Id).ConfigureAwait(true);
        splitEntity!.State = SplitState.ReadyToTest;

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
    }
}
