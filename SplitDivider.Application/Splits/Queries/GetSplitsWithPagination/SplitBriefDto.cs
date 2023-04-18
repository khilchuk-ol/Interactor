using SplitDivider.Application.Common.Mappings;

namespace SplitDivider.Application.Splits.Queries.GetSplitsWithPagination;

public class SplitBriefDto : IMapFrom<Split>
{
    public int Id { get; init; }

    public string Name { get; set; }
    
    public int State { get; set; }
}