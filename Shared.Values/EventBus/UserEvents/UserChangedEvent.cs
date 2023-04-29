using Newtonsoft.Json;
using Shared.Values.EventBus.Common;

namespace Shared.Values.EventBus.UserEvents;

[JsonObject(MemberSerialization.OptOut)]
public class UserChangedEvent : BaseEvent
{
    public int Id { get; set; }
    
    public int? State { get; set; }
    
    public string? Gender { get; set; }
    
    public int? CountryId { get; set; }
    
    public override UserChangedEvent FromEventData(string eventData)
    {
        var obj = JsonConvert.DeserializeObject<UserChangedEvent>(eventData);

        return obj;
    }

    public override string GetEventType()
    {
        return "user.changed.event";
    }
}