using Microsoft.AspNetCore.Identity;
using SplitDivider.Infrastructure.Identity;
using SplitDivider.WebApp.Authorization.Models;

namespace SplitDivider.WebApp.Controllers;

public class AuthController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    
    private readonly SignInManager<ApplicationUser> _signInManager;
 
    public AuthController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [Microsoft.AspNetCore.Mvc.HttpGet("me")]
    public async Task<ApplicationUser?> GeCurrenttUser()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);

        return user;
    }
    
    [Microsoft.AspNetCore.Mvc.HttpPost("signup")]
    public async Task<Response?> Register(RegistrationModel model)
    {
        if(ModelState.IsValid)
        {
            var user = new ApplicationUser { Email = model.Email, UserName = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, true);

                return new Response
                {
                    Result = user
                };
            }

            return new Response
            {
                Errors = result.Errors,
            };
        }

        return null;
    }
    
    [Microsoft.AspNetCore.Mvc.HttpPost("login")]
    public async Task<ApplicationUser?> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var signedUser = await _userManager.FindByEmailAsync(model.Email);

            if (signedUser == null) throw new UnauthorizedAccessException();

            var result = await _signInManager.PasswordSignInAsync(
                signedUser.UserName, 
                model.Password, 
                model.RememberMe, 
                false);

            if (!result.Succeeded) throw new UnauthorizedAccessException();

            return signedUser;
        }

        return null;
    }
 
    [Microsoft.AspNetCore.Mvc.HttpPost("signout")]
    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
        HttpContext.Session.Clear();
    }
}