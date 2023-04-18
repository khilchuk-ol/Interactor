using MediatR;
using Shared.Values.Enums;
using Shared.Values.EventBus.Common;
using Shared.Values.EventBus.UserEvents;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Application.Users.Commands.UpdateUser;

namespace SplitDivider.WebApp.EventBus.Handlers.UserEvents;

public class UserChangedEventHandler : IEventBusEventHandler
{
    private ISender _mediator;
    
    public UserChangedEventHandler(ISender mediator)
    {
        _mediator = mediator;
    }

    public Task Handle(BaseEvent eEvent)
    {
        var eventData = (UserChangedEvent)eEvent;

        return _mediator.Send(new UpdateUserCommand
        {
            Id = eventData.Id,
            CountryId = eventData.CountryId,
            Gender = eventData.Gender,
            State = eventData.State.HasValue ? (UserState)eventData.State : null
        });
    }
}