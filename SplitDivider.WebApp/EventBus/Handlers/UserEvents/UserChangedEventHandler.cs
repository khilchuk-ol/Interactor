using MediatR;
using Shared.Values.Enums;
using Shared.Values.EventBus.Common;
using Shared.Values.EventBus.UserEvents;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Application.Users.Commands.UpdateUser;

namespace SplitDivider.WebApp.EventBus.Handlers.UserEvents;

public class UserChangedEventHandler : IEventBusEventHandler
{
    private IServiceProvider _services;
    
    public UserChangedEventHandler(IServiceProvider services)
    {
        _services = services;
    }

    public async Task Handle(BaseEvent eEvent)
    {
        var eventData = (UserChangedEvent)eEvent;
        
        using var scope = _services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<ISender>();
        
        if (mediator == null)
        {
            throw new Exception("mediator is not found");
        }

        await mediator.Send(new UpdateUserCommand
        {
            Id = eventData.Id,
            CountryId = eventData.CountryId,
            Gender = eventData.Gender,
            State = eventData.State.HasValue ? (UserState)eventData.State : null
        });
    }
}