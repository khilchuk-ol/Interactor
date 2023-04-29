using System.Globalization;
using MediatR;
using Shared.Values.Enums;
using SplitDivider.Application.Common.Interfaces;

namespace SplitDivider.Application.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest
{
    public int Id { get; init; }
    
    public int CountryId { get; init; }

    public string Gender { get; init; }
    
    public string RegDt { get; init; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = new User
        {
            Id = request.Id,
            State = UserState.Registered,
            CountryId = request.CountryId,
            Gender = request.Gender,
            RegistrationDt = DateTime.Parse(request.RegDt, CultureInfo.InvariantCulture)
        };

        _context.AppUsers.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}