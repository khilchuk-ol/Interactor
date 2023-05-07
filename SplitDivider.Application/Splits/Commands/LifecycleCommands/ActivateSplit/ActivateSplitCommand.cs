using MediatR;
using SplitDivider.Application.Common.Exceptions;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Domain.Enums;
using SplitDivider.Domain.Events;
using InvalidOperationException = SplitDivider.Application.Common.Exceptions.InvalidOperationException;

namespace SplitDivider.Application.Splits.Commands.LifecycleCommands.ActivateSplit;

public record ActivateSplitCommand(int Id) : IRequest;

public class ActivateSplitCommandHandler : IRequestHandler<ActivateSplitCommand>
{
    private readonly IApplicationDbContext _context;

    public ActivateSplitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ActivateSplitCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Splits
            .FindAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Split), request.Id);
        }

        if (!Split.ActivatableStates.Contains(entity.State))
        {
            throw new InvalidOperationException("Split can only be activated in Created or Suspended status");
        }

        entity.State = SplitState.Activated;
        
        entity.AddDomainEvent(new SplitActivatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
    }
}