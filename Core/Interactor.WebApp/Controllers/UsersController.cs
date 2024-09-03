using Interactor.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Interactor.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userSvc;

    private IInteractionService _interactionSvc;
    
    public UsersController(
        IUserService userSvc,
        IInteractionService interactionSvc)
    {
        _userSvc = userSvc;
        _interactionSvc = interactionSvc;
    }
    
    [HttpPost("register")]
    public async Task SendRandomUserRegistered([FromQuery(Name = "count")] int count)
    {
        if (count == 0) count++;
        
        for (var i = 0; i < count; i++)
        {
            await _userSvc.RegisterRandomUser();
        }
    }
    
    [HttpPost("ban")]
    public async Task SendRandomUserBanned()
    {
        await _userSvc.BanRandomUser();
    }
    
    [HttpPost("interacted")]
    public async Task SendRandomUsersInteracted([FromQuery(Name = "count")] int count)
    {
        if (count == 0) count++;
        
        for (var i = 0; i < count; i++)
        {
            await _interactionSvc.HandleRandomUsersInteracted();
        }
    }
}