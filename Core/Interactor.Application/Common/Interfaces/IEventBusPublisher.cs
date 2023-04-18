using Shared.Values.EventBus.Common;

namespace Interactor.Application.Common.Interfaces;

public interface IEventBusPublisher
{
    public void Send(BaseEvent eEvent);
}