using MediatR;
using SplitDivider.Application.Common.Exceptions;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Domain.Enums;
using Z.EntityFramework.Plus;
using InvalidOperationException = SplitDivider.Application.Common.Exceptions.InvalidOperationException;

namespace SplitDivider.Application.Splits.Commands.LifecycleCommands.SuspendSplit;

public record SuspendSplitCommand(int Id) : IRequest;

public class SuspendSplitCommandHandler : IRequestHandler<SuspendSplitCommand>
{
    private readonly IApplicationDbContext _context;

    public SuspendSplitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(SuspendSplitCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Splits
            .FindAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Split), request.Id);
        }
        
        if (!Split.SuspendableStates.Contains(entity.State))
        {
            throw new InvalidOperationException("Split can only be suspended after activation");
        }

        entity.State = SplitState.Suspended;

        await _context.UserSplits
            .Where(ug => ug.SplitId == entity.Id)
            .DeleteAsync(cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}
