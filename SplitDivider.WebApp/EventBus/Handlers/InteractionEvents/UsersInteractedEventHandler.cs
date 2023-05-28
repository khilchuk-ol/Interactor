using MediatR;
using Shared.Values.EventBus.Common;
using Shared.Values.EventBus.InteractionEvents;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Application.Relations.Commands.CreateRelation;

namespace SplitDivider.WebApp.EventBus.Handlers.InteractionEvents;

public class UsersInteractedEventHandler : IEventBusEventHandler
{
    private IServiceProvider _services;
    
    public UsersInteractedEventHandler(IServiceProvider services)
    {
        _services = services;
    }

    public async Task Handle(BaseEvent eEvent)
    {
        var eventData = (UsersInteractedEvent)eEvent;
        
        using var scope = _services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<ISender>();
        
        if (mediator == null)
        {
            throw new Exception("mediator is not found");
        }

        await mediator.Send(new CreateRelationCommand
        {
            UserId = eventData.UserId,
            ContactId = eventData.ContactId,
            InteractionType = eventData.Interaction
        }).ConfigureAwait(true);
    }
}