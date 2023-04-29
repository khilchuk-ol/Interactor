using System.Globalization;
using FluentValidation;
using Shared.Values.ValueObjects;

namespace SplitDivider.Application.Splits.Commands.CreateSplit;

public class CreateSplitCommandValidator : AbstractValidator<CreateSplitCommand>
{
    public CreateSplitCommandValidator()
    {
        RuleFor(s => s.Name)
            .MaximumLength(30)
            .MinimumLength(5)
            .NotEmpty()
            .WithMessage("Split name must have length between 5 and 30 characters");

        RuleFor(s => s.ActionsWeights)
            .NotEmpty()
            .WithMessage("Actions weights must not be empty");

        RuleForEach(s => s.ActionsWeights)
            .Must(p => InteractionType.IsSupported(p.Key) && p.Value >= 0);

        RuleFor(s => s.Gender)
            .Must(Gender.IsSupported!)
            .When(s => s.Gender != null);

        RuleFor(s => s.MinRegDt)
            .Must(dt => DateTime.TryParse(dt, CultureInfo.InvariantCulture, out _))
            .When(s => s.MinRegDt != null);

        RuleFor(s => s.CountryIds)
            .Must(cIds => cIds!.All(id => id >= 0))
            .When(s => s.CountryIds != null);
    }
}