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
    public async Task SendRandomUserRegistered()
    {
        await _userSvc.RegisterRandomUser();
    }
    
    [HttpPost("ban")]
    public async Task SendRandomUserBanned()
    {
        await _userSvc.BanRandomUser();
    }
    
    [HttpPost("interacted")]
    public async Task SendRandomUsersInteracted()
    {
        await _interactionSvc.HandleRandomUsersInteracted();
    }
}