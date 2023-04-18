using FluentValidation;
using Shared.Values.ValueObjects;

namespace SplitDivider.Application.Relations.Commands.CreateRelation;

public class CreateRelationCommandValidator : AbstractValidator<CreateRelationCommand>
{
    public CreateRelationCommandValidator()
    {
        RuleFor(u => u.InteractionType)
            .Must(InteractionType.IsSupported);
    }
}