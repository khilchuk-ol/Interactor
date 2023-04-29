using System.Globalization;
using FluentValidation;
using Shared.Values.ValueObjects;

namespace SplitDivider.Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(u => u.Gender)
            .Must(Gender.IsSupported);

        RuleFor(u => u.RegDt)
            .Must(dt => DateTime.TryParse(dt, CultureInfo.InvariantCulture, out _));

        RuleFor(u => u.CountryId)
            .Must(cid => cid >= 0);
    }
}