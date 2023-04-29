using Newtonsoft.Json;
using Shared.Values.EventBus.Common;

namespace Shared.Values.EventBus.InteractionEvents;

[JsonObject(MemberSerialization.OptOut)]
public class UsersInteractedEvent : BaseEvent
{
    public int UserId { get; set; }
    
    public int ContactId { get; set; }
    
    public string Interaction { get; set; }
    
    public string Dt { get; set; }
    
    public override UsersInteractedEvent FromEventData(string eventData)
    {
        var obj = JsonConvert.DeserializeObject<UsersInteractedEvent>(eventData);

        return obj;
    }

    public override string GetEventType()
    {
        return "user.interacted.event";
    }
}