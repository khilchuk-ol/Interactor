using FluentValidation;
using Shared.Values.ValueObjects;

namespace SplitDivider.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(u => u.Gender)
            .Must(Gender.IsSupported!)
            .When(u => u.Gender != null);

        RuleFor(u => u.CountryId)
            .Must(cid => cid >= 0)
            .When(u => u.CountryId.HasValue);

        RuleFor(u => u.State)
            .IsInEnum()
            .When(u => u.State.HasValue);
    }
}