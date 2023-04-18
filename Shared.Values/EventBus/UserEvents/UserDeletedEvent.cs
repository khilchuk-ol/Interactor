using System.Text.Json;
using System.Text.Json.Serialization;
using Shared.Values.EventBus.Common;

namespace Shared.Values.EventBus.UserEvents;

public class UserDeletedEvent : BaseEvent
{
    public int Id { get; set; }
    
    [JsonIgnore]
    public Keys ContentType { get; }
    
    public override string GetEventType()
    {
        return "user.deleted.event";
    }
    
    public override string ToEventData()
    {
        return JsonSerializer.Serialize(this);
    }
    
    public override string GetContentType()
    {
        return ContentType.ToString();
    }

    public override UserDeletedEvent FromEventData(string eventData)
    {
        var obj = JsonSerializer.Deserialize<UserDeletedEvent>(eventData);

        return obj;
    }
}