using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySql.EntityFrameworkCore.Extensions;
using SplitDivider.Infrastructure.Identity;

namespace SplitDivider.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitializer(
        ILogger<ApplicationDbContextInitializer> logger, 
        ApplicationDbContext context, 
        UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsMySql())
            {
                await _context.Database.MigrateAsync().ConfigureAwait(true);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync().ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var defaultRoles = new[]
        {
            new IdentityRole("Administrator"),
            new IdentityRole("Experimenter"),
        };

        foreach (var role in defaultRoles)
        {
            if (_roleManager.Roles.All(r => r.Name != role.Name))
            {
                await _roleManager.CreateAsync(role).ConfigureAwait(true);
            }
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "admin@localhost", Email = "admin@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!").ConfigureAwait(true);

            var rolesToAdd = new List<string>();

            foreach (var role in defaultRoles)
            {
                if (!string.IsNullOrWhiteSpace(role.Name))
                {
                    rolesToAdd.Add(role.Name);
                }
            }
            
            if (rolesToAdd.Count > 0)
            {
                await _userManager.AddToRolesAsync(administrator, rolesToAdd).ConfigureAwait(true);
            }
        }
    }
}