namespace SplitDivider.Domain.Entities;

public class Split : BaseAuditableEntity
{
    public static readonly SplitState[] EditableStates = { SplitState.Created, SplitState.Suspended };

    public static readonly SplitState[] ActivatableStates = { SplitState.Created, SplitState.Suspended};

    public static readonly SplitState[] SuspendableStates = { SplitState.Activated, SplitState.ReadyToTest };
    
    public static readonly SplitState[] ClosableStates = { SplitState.Created, SplitState.ReadyToTest, SplitState.Suspended };

    private Gender? _gender;

    public string Name { get; set; } = "None";
    
    public SplitState State { get; set; }
    
    public Dictionary<InteractionType, int> ActionsWeights { get; set; } = new();

    public List<int>? CountryIds { get; set; }
    
    public string? Gender {
        get => _gender?.Name;
        set => _gender = value == null ? null : Shared.Values.ValueObjects.Gender.From(value);
    }
    
    public DateTime? MinRegistrationDt { get; set; }
}