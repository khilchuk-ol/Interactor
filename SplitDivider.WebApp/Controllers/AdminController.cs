using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Infrastructure.Identity;

namespace SplitDivider.WebApp.Controllers;

//[Authorize(Roles = "Administrator")]
public class AdminController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly IIdentityService _identityService;
    
    public AdminController(
        IIdentityService identityService,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _identityService = identityService;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet("users")]
    public async Task<List<ApplicationUser>> GetUsers()
    {
        return await _userManager.Users.ToListAsync().ConfigureAwait(true);
    }
    
    [HttpGet("users/{email}")]
    public async Task<IActionResult> GetUser(string email)
    {
        var user = await _userManager.Users
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync()
            .ConfigureAwait(true);
        if (user == null)
        {
            return NotFound();
        }

        var roleNames = await _userManager.GetRolesAsync(user).ConfigureAwait(true);
        var roles = new List<IdentityRole>();

        if (roleNames != null)
        {
            roles = await _roleManager.Roles
                .Where(r => roleNames.Contains(r.Name))
                .ToListAsync()
                .ConfigureAwait(true);
        }

        return Ok(new
        {
            user,
            roles
        });
    }
    
    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync().ConfigureAwait(true);
        
        return Ok(roles);
    }
    
    [HttpDelete("user/{userId}/role/{role}")]
    public async Task<IActionResult> DeleteRole(string userId, string role)
    {
        var res = await _identityService.DeleteRoleForUser(userId, role).ConfigureAwait(true);
        
        return res ? Ok() : BadRequest();
    }
    
    [HttpPut("user/{userId}/role/{role}")]
    public async Task<IActionResult> AddRole(string userId, string role)
    {
        var res = await _identityService.AddRoleForUser(userId, role).ConfigureAwait(true);

        return res ? Ok() : BadRequest();
    }
}