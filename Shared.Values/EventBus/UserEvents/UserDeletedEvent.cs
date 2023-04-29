using Newtonsoft.Json;
using Shared.Values.EventBus.Common;

namespace Shared.Values.EventBus.UserEvents;

[JsonObject(MemberSerialization.OptOut)]
public class UserDeletedEvent : BaseEvent
{
    public int Id { get; set; }
    
    public override UserDeletedEvent FromEventData(string eventData)
    {
        var obj = JsonConvert.DeserializeObject<UserDeletedEvent>(eventData);

        return obj;
    }

    public override string GetEventType()
    {
        return "user.deleted.event";
    }
}