namespace Shared.Values.Exceptions;

public class UnsupportedGenderException : Exception
{
    public UnsupportedGenderException(string name) : base($"Gender \"{name}\" is unsupported.")
    {
    }
}