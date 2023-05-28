using MediatR;
using Shared.Values.EventBus.Common;
using Shared.Values.EventBus.UserEvents;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Application.Users.Commands.CreateUser;

namespace SplitDivider.WebApp.EventBus.Handlers.UserEvents;

public class UserRegisteredEventHandler : IEventBusEventHandler
{
    private IServiceProvider _services;
    
    public UserRegisteredEventHandler(IServiceProvider services)
    {
        _services = services;
    }

    public async Task Handle(BaseEvent eEvent)
    {
        var eventData = (UserRegisteredEvent)eEvent;

        using var scope = _services.CreateScope();
        var mediator = scope.ServiceProvider.GetService<ISender>();
        
        if (mediator == null)
        {
            throw new Exception("mediator is not found");
        }

        await mediator.Send(new CreateUserCommand
        {
            Id = eventData.Id,
            CountryId = eventData.CountryId,
            Gender = eventData.Gender,
            RegDt = eventData.RegDt
        }).ConfigureAwait(true);
    }
}