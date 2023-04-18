using MediatR;
using Shared.Values.ValueObjects;
using SplitDivider.Application.Common.Exceptions;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Domain.Enums;
using InvalidOperationException = SplitDivider.Application.Common.Exceptions.InvalidOperationException;

namespace SplitDivider.Application.Splits.Commands.UpdateSplit;

public record UpdateSplitCommand : IRequest
{
    public int Id { get; init; }
    
    public string? Name { get; init; }
    
    public IDictionary<string, int>? ActionsWeights { get; set; }
    
    public IList<int>? CountryIds { get; set; }
    
    public string? Gender { get; init; }
    
    public string? MinRegDt { get; init; }
}

public class UpdateSplitCommandHandler : IRequestHandler<UpdateSplitCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSplitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateSplitCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Splits
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Split), request.Id);
        }

        if (!Split.EditableStates.Contains(entity.State))
        {
            throw new InvalidOperationException("Invalid split status for update: split can only be updated before activation");
        }
        
        if (request.Name != null)
        {
            entity.Name = request.Name;
        }
        
        if (request.ActionsWeights != null)
        {
            entity.ActionsWeights = request.ActionsWeights.ToDictionary(p => InteractionType.From(p.Key), p => p.Value);
        }
        
        if (request.Gender != null)
        {
            entity.Gender = request.Gender;
        }
        
        if (request.CountryIds != null)
        {
            entity.CountryIds = request.CountryIds.ToList();
        }
        
        if (request.MinRegDt != null)
        {
            entity.MinRegistrationDt = DateTime.Parse(request.MinRegDt);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}