namespace Shared.Values.ValueObjects;

public sealed class Gender : ValueObjectWithName
{
    private Gender(string name) : base(name)
    {
    }

    public static Gender From(string name)
    {
        var gender = new Gender(name);

        if (!SupportedGenders.Contains(gender))
        {
            throw new UnsupportedGenderException(name);
        }

        return gender;
    }
    
    public static bool IsSupported(string name)
    {
        var action = new Gender(name);

        return SupportedGenders.Contains(action);
    }
    
    public static implicit operator string(Gender gender)
    {
        return gender.ToString();
    }

    public static explicit operator Gender(string name)
    {
        return From(name);
    }
    
    public static Gender Male = new("male");

    public static Gender Female = new("female");
    
    private static IEnumerable<Gender> SupportedGenders
    {
        get
        {
            yield return Male;
            yield return Female;
        }
    }
}