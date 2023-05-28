using MediatR;
using Shared.Values.EventBus.Common;
using Shared.Values.EventBus.UserEvents;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Application.Users.Commands.DeleteUser;

namespace SplitDivider.WebApp.EventBus.Handlers.UserEvents;

public class UserDeletedEventHandler : IEventBusEventHandler
{
    private IServiceProvider _services;
    
    public UserDeletedEventHandler(IServiceProvider services)
    {
        _services = services;
    }

    public async Task Handle(BaseEvent eEvent)
    {
        var eventData = (UserDeletedEvent)eEvent;
        
        using var scope = _services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<ISender>();
        
        if (mediator == null)
        {
            throw new Exception("mediator is not found");
        }

        await mediator.Send(new DeleteUserCommand(eventData.Id)).ConfigureAwait(true);
    }
}