using SplitDivider.Application.Common.Mappings;

namespace SplitDivider.Application.Users.Queries.GetUserSplits;

public class UserSplitDto : IMapFrom<UserSplit>
{
    public int SplitId { get; set; }
    
    public int Group { get; set; }
}