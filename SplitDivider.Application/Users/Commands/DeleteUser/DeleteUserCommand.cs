using MediatR;
using SplitDivider.Application.Common.Exceptions;
using SplitDivider.Application.Common.Interfaces;

namespace SplitDivider.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(int Id) : IRequest;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        
        var entity = await _context.AppUsers
            .FindAsync(request.Id, cancellationToken)
            .ConfigureAwait(true);

        if (entity == null)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        _context.AppUsers.Remove(entity);
        
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
    }

}