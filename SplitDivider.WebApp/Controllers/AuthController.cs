using MediatR.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitDivider.Infrastructure.Identity;
using SplitDivider.Infrastructure.Services;
using SplitDivider.WebApp.Authorization.Models;

namespace SplitDivider.WebApp.Controllers;

public class AuthController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    
    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly AuthService _authService;
    
    private readonly JwtFactory _jwtFactory;
 
    public AuthController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager,
        AuthService authService,
        JwtFactory jwtFactory
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
        _jwtFactory = jwtFactory;
    }

    [HttpPost("login")]
    public async Task<AuthUserDTO?> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var dto = await _authService.Authorize(model.Email, model.Password, model.RememberMe);

            return dto;
        }

        return null;
    }
 
    [HttpPost("signout")]
    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    [HttpGet("me")]
    public async Task<ApplicationUser?> GetUser()
    {
        if (!Request.Headers.ContainsKey("x-auth-token"))
        {
            throw new UnauthorizedAccessException();
        }

        var token = Request.Headers["x-auth-token"].ToString();
        
        var userId = _jwtFactory.GetValueFromToken(token);

        return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    [HttpPost("signup")]
    public async Task<AuthUserDTO?> TestRegister(RegistrationModel model)
    {
        if(ModelState.IsValid)
        {
            var user = new ApplicationUser { Email = model.Email, UserName = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);


            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException();
            }
            
            var dto = await _authService.Authorize(model.Email, model.Password, true);
            
            return dto;
        }

        return null;
    }
}