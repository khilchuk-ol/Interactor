namespace SplitDivider.Domain.Entities;

public class UserSplit
{
    public static readonly int GroupControl = 0;

    public static readonly int GroupTest = 1;
    
    public int UserId { get; set; }
    
    public int SplitId { get; set; }
    
    public int Group { get; set; }
}