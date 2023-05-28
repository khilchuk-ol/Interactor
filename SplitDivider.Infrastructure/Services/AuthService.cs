using MediatR.Dto;
using Microsoft.AspNetCore.Identity;
using SplitDivider.Infrastructure.Identity;
using SplitDivider.Infrastructure.Persistence;

namespace SplitDivider.Infrastructure.Services;

public class AuthService
{
    private readonly ApplicationDbContext _context;
    
    private readonly JwtFactory _jwtFactory;
    
    private readonly UserManager<ApplicationUser> _userManager;
    
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthService(
        ApplicationDbContext context, 
        JwtFactory jwtFactory, 
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _jwtFactory = jwtFactory;
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<AuthUserDTO> Authorize(string email, string password, bool rememberMe)
    {
        var signedUser = await _userManager.FindByEmailAsync(email).ConfigureAwait(true);

        if (signedUser == null) throw new UnauthorizedAccessException("Could not authorize user. Incorrect email");
        
        var result = await _signInManager.PasswordSignInAsync(
            signedUser.UserName, 
            password, 
            rememberMe, 
            false)
            .ConfigureAwait(true);

        if (!result.Succeeded) throw new UnauthorizedAccessException("Could not authorize user. Incorrect password");
        
        string accessToken = await _jwtFactory
            .GenerateAccessToken(signedUser.Id, signedUser.UserName, signedUser.Email)
            .ConfigureAwait(true);
        
        return new AuthUserDTO
        {
            Token = accessToken,
            User = signedUser,
        };
    }
}