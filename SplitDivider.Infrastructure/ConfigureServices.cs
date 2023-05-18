using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.Infrastructure.Identity;
using SplitDivider.Infrastructure.Persistence;
using SplitDivider.Infrastructure.Persistence.Interceptors;
using SplitDivider.Infrastructure.Services;

namespace SplitDivider.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("SplitDividerDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySQL(configuration.GetConnectionString("DefaultConnection")!,
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }
        
        services.AddScoped<IApplicationDbContext>(provider =>
        {
            var context =  provider.GetRequiredService<ApplicationDbContext>();

            context.Database.EnsureCreated();

            return context;
        });
        
        services.AddScoped<ApiAuthorizationDbContext<ApplicationUser>>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitializer>();

        services
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        // services.AddIdentityServer()
        //     .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
        
        // services.AddAuthentication()
        //     .AddIdentityCookies();
        
        services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
        });
        
        services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = false;
            options.ExpireTimeSpan = TimeSpan.FromHours(5);

            options.Events = new CookieAuthenticationEvents
            {
                OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = 403;
                    return Task.FromResult(0);
                },
                OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.FromResult(0);
                }
            };
            
            options.SlidingExpiration = true;
        });

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();

        // services.AddAuthorization(options =>
        //     options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        return services;
    }
}
