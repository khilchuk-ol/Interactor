using FluentValidation;
using SplitDivider.Domain.Enums;

namespace SplitDivider.Application.Splits.Queries.GetSplitsWithPagination;

public class GetSplitsWithPaginationQueryValidator : AbstractValidator<GetSplitsWithPaginationQuery>
{
    public GetSplitsWithPaginationQueryValidator()
    {
        When(x => x.State.HasValue, () =>
        {
            RuleFor(x => (SplitState)x.State!.Value)
                .IsInEnum()
                .WithMessage("Split state must be a valid value");
        });

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
