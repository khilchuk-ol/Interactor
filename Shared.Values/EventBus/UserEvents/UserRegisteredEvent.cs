using Newtonsoft.Json;
using Shared.Values.EventBus.Common;

namespace Shared.Values.EventBus.UserEvents;

[JsonObject(MemberSerialization.OptOut)]
public class UserRegisteredEvent : BaseEvent
{
    public int Id { get; set; }
    
    public string Gender { get; set; }
    
    public int CountryId { get; set; }
    
    public string RegDt { get; set; }
    
    public override UserRegisteredEvent FromEventData(string eventData)
    {
        var obj = JsonConvert.DeserializeObject<UserRegisteredEvent>(eventData);

        return obj;
    }

    public override string GetEventType()
    {
        return "user.registered.event";
    }
}