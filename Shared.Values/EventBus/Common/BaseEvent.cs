using Newtonsoft.Json;

namespace Shared.Values.EventBus.Common;

[JsonObject(MemberSerialization.OptOut)]
public abstract class BaseEvent
{
    public abstract string GetEventType();

    public string ToEventData()
    {
        return JsonConvert.SerializeObject(this);
    }

    public abstract BaseEvent FromEventData(string eventData);
}