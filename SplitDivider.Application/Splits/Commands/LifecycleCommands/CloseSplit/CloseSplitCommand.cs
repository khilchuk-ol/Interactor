using MediatR;
using SplitDivider.Application.Common.Exceptions;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Domain.Enums;
using InvalidOperationException = SplitDivider.Application.Common.Exceptions.InvalidOperationException;

namespace SplitDivider.Application.Splits.Commands.LifecycleCommands.CloseSplit;

public record CloseSplitCommand(int Id) : IRequest;

public class CloseSplitCommandHandler : IRequestHandler<CloseSplitCommand>
{
    private readonly IApplicationDbContext _context;

    public CloseSplitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CloseSplitCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Splits
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Split), request.Id);
        }
        
        if (!Split.ClosableStates.Contains(entity.State))
        {
            throw new InvalidOperationException("Invalid split state for closing");
        }

        entity.State = SplitState.Closed;
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}
