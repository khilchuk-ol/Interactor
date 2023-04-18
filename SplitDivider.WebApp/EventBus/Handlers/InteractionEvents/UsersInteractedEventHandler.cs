using MediatR;
using Shared.Values.EventBus.Common;
using Shared.Values.EventBus.InteractionEvents;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Application.Relations.Commands.CreateRelation;

namespace SplitDivider.WebApp.EventBus.Handlers.InteractionEvents;

public class UsersInteractedEventHandler : IEventBusEventHandler
{
    private ISender _mediator;
    
    public UsersInteractedEventHandler(ISender mediator)
    {
        _mediator = mediator;
    }

    public Task Handle(BaseEvent eEvent)
    {
        var eventData = (UsersInteractedEvent)eEvent;

        return _mediator.Send(new CreateRelationCommand
        {
            UserId = eventData.UserId,
            ContactId = eventData.ContactId,
            InteractionType = eventData.Interaction
        });
    }
}