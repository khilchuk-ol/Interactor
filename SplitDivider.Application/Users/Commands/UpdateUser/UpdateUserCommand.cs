using MediatR;
using Shared.Values.Enums;
using SplitDivider.Application.Common.Exceptions;
using SplitDivider.Application.Common.Interfaces;

namespace SplitDivider.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand : IRequest
{
    public int Id { get; init; }
    
    public UserState? State { get; init; }
    
    public string? Gender { get; init; }
    
    public int? CountryId { get; init; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.AppUsers
            .FindAsync(request.Id, cancellationToken)
            .ConfigureAwait(true);

        if (entity == null)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        if (request.State.HasValue)
        {
            entity.State = request.State.Value;
        }

        if (request.Gender != null)
        {
            entity.Gender = request.Gender;
        }
        
        if (request.CountryId.HasValue)
        {
            entity.CountryId = request.CountryId.Value;
        }

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
    }
}