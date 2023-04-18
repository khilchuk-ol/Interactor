namespace Shared.Values.Exceptions;

public class UnsupportedInteractionTypeException : Exception
{
    public UnsupportedInteractionTypeException(string name) : base($"InteractionType \"{name}\" is unsupported.")
    {
    }
}
