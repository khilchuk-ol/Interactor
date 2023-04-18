using System.Text.Json;
using System.Text.Json.Serialization;
using Shared.Values.EventBus.Common;

namespace Shared.Values.EventBus.InteractionEvents;

public class UsersInteractedEvent : BaseEvent
{
    public int UserId { get; set; }
    
    public int ContactId { get; set; }
    
    public string Interaction { get; set; }
    
    public string Dt { get; set; }
    
    [JsonIgnore]
    public Keys ContentType { get; }
    
    public override string GetEventType()
    {
        return "user.interacted.event";
    }

    public override string ToEventData()
    {
        return JsonSerializer.Serialize(this);
    }

    public override string GetContentType()
    {
        return ContentType.ToString();
    }

    public override UsersInteractedEvent FromEventData(string eventData)
    {
        var obj = JsonSerializer.Deserialize<UsersInteractedEvent>(eventData);

        return obj;
    }
}