using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Infrastructure.Identity;

namespace SplitDivider.WebApp.Controllers;

[Authorize(Roles = "Administrator")]
public class AdminController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    
    private readonly IIdentityService _identityService;
    
    public AdminController(
        IIdentityService identityService,
        UserManager<ApplicationUser> userManager)
    {
        _identityService = identityService;
        _userManager = userManager;
    }

    [HttpGet("users")]
    public async Task<List<ApplicationUser>> GetUsers()
    {
        return await _userManager.Users.ToListAsync();
    }
    
    [HttpDelete("user/{userId}/role/{roleId}")]
    public async Task<bool> DeleteRole(string userId, string roleId)
    {
        return await _identityService.DeleteRoleForUser(userId, roleId);
    }
    
    [HttpPut("user/{userId}/role/{roleId}")]
    public async Task<bool> AddRole(string userId, string roleId)
    {
        return await _identityService.AddRoleForUser(userId, roleId);
    }
}