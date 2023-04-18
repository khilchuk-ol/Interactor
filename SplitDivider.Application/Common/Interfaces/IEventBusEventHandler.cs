using Shared.Values.EventBus.Common;

namespace SplitDivider.Application.Common.Interfaces;

public interface IEventBusEventHandler
{
    public Task Handle(BaseEvent eEvent);
}