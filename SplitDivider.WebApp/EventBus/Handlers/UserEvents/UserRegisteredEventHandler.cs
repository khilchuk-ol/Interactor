using MediatR;
using Shared.Values.EventBus.Common;
using Shared.Values.EventBus.UserEvents;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Application.Users.Commands.CreateUser;

namespace SplitDivider.WebApp.EventBus.Handlers.UserEvents;

public class UserRegisteredEventHandler : IEventBusEventHandler
{
    private ISender _mediator;
    
    public UserRegisteredEventHandler(ISender mediator)
    {
        _mediator = mediator;
    }

    public Task Handle(BaseEvent eEvent)
    {
        var eventData = (UserRegisteredEvent)eEvent;

        return _mediator.Send(new CreateUserCommand
        {
            Id = eventData.Id,
            CountryId = eventData.CountryId,
            Gender = eventData.Gender,
            RegDt = eventData.RegDt
        });
    }
}