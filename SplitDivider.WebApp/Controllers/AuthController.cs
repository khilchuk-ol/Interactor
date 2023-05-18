using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    
    [HttpPost("signup")]
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
    
    [HttpPost("login")]
    public async Task<bool> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, 
                model.Password, 
                model.RememberMe, 
                false);

            return result.Succeeded;
        }

        return false;
    }
 
    [HttpPost("signout")]
    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }
}