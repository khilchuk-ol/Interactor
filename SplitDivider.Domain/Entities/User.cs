namespace SplitDivider.Domain.Entities;

public class User : BaseEntity
{
    private Gender? _gender;
    
    public int CountryId { get; set; }
    
    public DateTime RegistrationDt { get; set; }
    
    public UserState State { get; set; }

    public string Gender {
        get => _gender?.Name ?? "undefined";
        set => _gender = Shared.Values.ValueObjects.Gender.From(value);
    }
}