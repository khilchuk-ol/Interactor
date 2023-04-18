using System.Text.Json;
using System.Text.Json.Serialization;
using Shared.Values.EventBus.Common;

namespace Shared.Values.EventBus.UserEvents;

public class UserRegisteredEvent : BaseEvent
{
    public int Id { get; set; }
    
    public string Gender { get; set; }
    
    public int CountryId { get; set; }
    
    public string RegDt { get; set; }
    
    [JsonIgnore]
    public Keys ContentType { get; }
    
    public override string GetEventType()
    {
        return "user.registered.event";
    }
    
    public override string ToEventData()
    {
        return JsonSerializer.Serialize(this);
    }
    
    public override string GetContentType()
    {
        return ContentType.ToString();
    }

    public override UserRegisteredEvent FromEventData(string eventData)
    {
        var obj = JsonSerializer.Deserialize<UserRegisteredEvent>(eventData);

        return obj;
    }
}