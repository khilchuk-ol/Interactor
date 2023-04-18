namespace Shared.Values.Common;

public abstract class ValueObjectWithName : ValueObject
{
    public string Name { get; protected set; } = "None";
    
    private ValueObjectWithName()
    {
    }

    protected ValueObjectWithName(string name)
    {
        Name = name;
    }
    
    public override string ToString()
    {
        return Name;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}