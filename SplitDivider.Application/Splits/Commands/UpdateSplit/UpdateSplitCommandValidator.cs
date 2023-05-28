using System.Globalization;
using FluentValidation;
using Shared.Values.ValueObjects;

namespace SplitDivider.Application.Splits.Commands.UpdateSplit;

public class UpdateSplitCommandValidator : AbstractValidator<UpdateSplitCommand>
{
    public UpdateSplitCommandValidator()
    {
        RuleFor(s => s.Name)
            .MaximumLength(30)
            .MinimumLength(5)
            .NotEmpty()
            .WithMessage("Split name must have length between 5 and 30 characters")
            .When(s => s.Name != null);

        RuleFor(s => s.ActionsWeights)
            .NotEmpty()
            .WithMessage("Actions weights must not be empty")
            .Custom((list, context) =>
            {
                if (list!.Any(p => !InteractionType.IsSupported(p.Key) || p.Value < 0))
                {
                    context.AddFailure("Invalid action weight rule");
                }
            })
            .When(s => s.ActionsWeights != null);
    }
}