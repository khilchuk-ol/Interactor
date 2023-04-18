using SplitDivider.Application.Common.Mappings;

namespace SplitDivider.Application.Users.Queries.GetSplitUsers;

public class SplitUserDto : IMapFrom<UserSplit>
{
    public int UserId { get; set; }
    
    public int Group { get; set; }
}