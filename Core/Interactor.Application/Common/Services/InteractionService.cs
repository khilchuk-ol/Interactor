using Interactor.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Values.EventBus.InteractionEvents;
using Shared.Values.ValueObjects;

namespace Interactor.Application.Common.Services;

public class InteractionService : IInteractionService
{
    private IApplicationDbContext _context;

    private IEventBusPublisher _publisher;
    
    public InteractionService(
        IApplicationDbContext context,
        IEventBusPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }
    
    public async Task HandleRandomUsersInteracted()
    {
        var minId = await _context.Users.MinAsync(u => u.Id);
        var maxId = await _context.Users.MaxAsync(u => u.Id);

        var random = new Random();

        var userId = random.Next(minId, maxId + 1);
        var contactId = random.Next(minId, maxId + 1);

        var interactionId = random.Next(0, InteractionType.SupportedActions.Count());

        var eEvent = new UsersInteractedEvent
        {
            UserId = userId,
            ContactId = contactId,
            Dt = DateTime.Now.ToString(),
            Interaction = InteractionType.SupportedActions.ElementAt(interactionId).Name
        };
        
        _publisher.Send(eEvent);
    }
}