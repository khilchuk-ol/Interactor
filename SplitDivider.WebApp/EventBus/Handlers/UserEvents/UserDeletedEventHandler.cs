using MediatR;
using Shared.Values.EventBus.Common;
using Shared.Values.EventBus.UserEvents;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Application.Users.Commands.DeleteUser;

namespace SplitDivider.WebApp.EventBus.Handlers.UserEvents;

public class UserDeletedEventHandler : IEventBusEventHandler
{
    private ISender _mediator;
    
    public UserDeletedEventHandler(ISender mediator)
    {
        _mediator = mediator;
    }

    public Task Handle(BaseEvent eEvent)
    {
        var eventData = (UserDeletedEvent)eEvent;

        return _mediator.Send(new DeleteUserCommand(eventData.Id));
    }
}