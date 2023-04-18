namespace Shared.Values.EventBus.Common;

public abstract class BaseEvent
{
    public abstract string GetEventType();

    public abstract string ToEventData();

    public abstract string GetContentType();

    public abstract BaseEvent FromEventData(string eventData);
}