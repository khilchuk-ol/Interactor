using MediatR;
using SplitDivider.Application.Common.Exceptions;
using SplitDivider.Application.Common.Interfaces;

namespace SplitDivider.Application.Splits.Commands.DeleteSplit;

public record DeleteSplitCommand(int Id) : IRequest;

public class DeleteSplitCommandHandler : IRequestHandler<DeleteSplitCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteSplitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSplitCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Splits
            .FindAsync(request.Id, cancellationToken)
            .ConfigureAwait(true);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Split), request.Id);
        }

        _context.Splits.Remove(entity);
        
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
    }

}