namespace Shared.Values.ValueObjects;

public sealed class InteractionType : ValueObjectWithName
{
    private InteractionType(string name) : base(name)
    {
    }
    
    public static InteractionType From(string name)
    {
        var action = new InteractionType(name);

        if (!SupportedActions.Contains(action))
        {
            throw new UnsupportedInteractionTypeException(name);
        }

        return action;
    }
    
    public static bool IsSupported(string name)
    {
        var action = new InteractionType(name);

        return SupportedActions.Contains(action);
    }
    
    public static implicit operator string(InteractionType interactionType)
    {
        if (interactionType == null) throw new ArgumentNullException(nameof(interactionType));
        
        return interactionType.ToString();
    }

    public static explicit operator InteractionType(string name)
    {
        return From(name);
    }

    public static readonly InteractionType Dialog = new("dialog");
    
    public static readonly InteractionType Like = new("like");
    
    public static readonly InteractionType Hide = new("hide");

    public static readonly InteractionType VideoCall = new("video-call");
    
    public static IEnumerable<InteractionType> SupportedActions
    {
        get
        {
            yield return Dialog;
            yield return Like;
            yield return Hide;
            yield return VideoCall;
        }
    }
}