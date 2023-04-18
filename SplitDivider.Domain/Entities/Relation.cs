namespace SplitDivider.Domain.Entities;

public class Relation : BaseEntity
{
    public int UserId { get; set; }
    
    public int ContactId { get; set; }

    private InteractionType? _interaction;
    
    public DateTime Dt { get; set; }
    
    public string Interaction {
        get => _interaction?.Name ?? "undefined";
        set => _interaction = InteractionType.From(value);
    }
}