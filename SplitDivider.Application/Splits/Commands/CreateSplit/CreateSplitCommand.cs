using System.Globalization;
using MediatR;
using Shared.Values.ValueObjects;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Domain.Enums;

namespace SplitDivider.Application.Splits.Commands.CreateSplit;

public record CreateSplitCommand : IRequest<int>
{
    public string Name { get; init; }
    
    public IDictionary<string, int> ActionsWeights { get; set; }
    
    public IList<int>? CountryIds { get; set; }
    
    public string? Gender { get; init; }
    
    public string? MinRegDt { get; init; }
}

public class CreateSplitCommandHandler : IRequestHandler<CreateSplitCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateSplitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateSplitCommand request, CancellationToken cancellationToken)
    {
        var entity = new Split
        {
            Name = request.Name,
            State = SplitState.Created,
            ActionsWeights = request.ActionsWeights.ToDictionary(p => InteractionType.From(p.Key), p => p.Value),
            CountryIds = request.CountryIds?.ToList(),
            Gender = request.Gender,
            MinRegistrationDt = request.MinRegDt != null ? DateTime.Parse(request.MinRegDt, CultureInfo.InvariantCulture) : null
        };

        _context.Splits.Add(entity);

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

        return entity.Id;
    }
}