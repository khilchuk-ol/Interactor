using System.Text.Json;
using System.Text.Json.Serialization;
using Shared.Values.EventBus.Common;

namespace Shared.Values.EventBus.UserEvents;

public class UserChangedEvent : BaseEvent
{
    public int Id { get; set; }
    
    public int? State { get; set; }
    
    public string? Gender { get; set; }
    
    public int? CountryId { get; set; }
    
    [JsonIgnore]
    public Keys ContentType { get; }
    
    public override string GetEventType()
    {
        return "user.changed.event";
    }
    
    public override string ToEventData()
    {
        return JsonSerializer.Serialize(this);
    }
    
    public override string GetContentType()
    {
        return ContentType.ToString();
    }

    public override UserChangedEvent FromEventData(string eventData)
    {
        var obj = JsonSerializer.Deserialize<UserChangedEvent>(eventData);

        return obj;
    }
}