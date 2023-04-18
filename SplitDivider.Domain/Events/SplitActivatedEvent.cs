using SplitDivider.Domain.Entities;

namespace SplitDivider.Domain.Events;

public class SplitActivatedEvent : BaseEvent
{
    public Split Split { get; }

    public SplitActivatedEvent(Split split)
    {
        Split = split;
    }
}
