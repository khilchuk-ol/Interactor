using System.Globalization;
using Interactor.Application.Common.Interfaces;
using Interactor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Values.Enums;
using Shared.Values.EventBus.UserEvents;
using Shared.Values.ValueObjects;

namespace Interactor.Application.Common.Services;

public class UserService : IUserService
{
    private IApplicationDbContext _context;

    private IEventBusPublisher _publisher;
    
    public UserService(
        IApplicationDbContext context,
        IEventBusPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }
    
    public async Task RegisterRandomUser()
    {
        var random = new Random();

        var genderId = random.Next(1, 3);
        var gender = genderId == 1 ? Gender.Male : Gender.Female;

        var countryId = random.Next(1, 50);

        var user = new User
        {
            CountryId = countryId,
            Gender = gender,
            RegistrationDt = DateTime.Now,

            State = UserState.Activated
        };

        var entity = await _context.Users.AddAsync(user);

        var eEvent = new UserRegisteredEvent
        {
            Id = entity.Entity.Id,
            CountryId = user.CountryId,
            Gender = user.Gender,
            RegDt = user.RegistrationDt.ToString(CultureInfo.InvariantCulture)
        };
        
        _publisher.Send(eEvent);
        
        Thread.Sleep(500);

        var activationEvent = new UserChangedEvent
        {
            Id = entity.Entity.Id,
            State = (int)UserState.Activated
        };
        
        _publisher.Send(activationEvent);
    }

    public async Task BanRandomUser()
    {
        var minId = await _context.Users.MinAsync(u => u.Id);
        var maxId = await _context.Users.MaxAsync(u => u.Id);

        var random = new Random();

        var userId = random.Next(minId, maxId + 1);

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return;
        }
        
        user.State = UserState.Banned;

        var eEvent = new UserChangedEvent
        {
            Id = userId,
            State = (int)UserState.Banned
        };
        
        _publisher.Send(eEvent);

        await _context.SaveChangesAsync(default);
    }
}