using Microsoft.AspNetCore.Mvc;

namespace Interactor.WebApp;

public static class ConfigureServices
{
    public static IServiceCollection AddWebAppServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();
        
        services.AddHttpContextAccessor();
        
        services.AddControllersWithViews();

        services.AddRazorPages();

        services.AddHealthChecks();
        
        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);
        
        services.AddOpenApiDocument((configure, _) =>
        {
            configure.Title = "Interactor API";
            configure.Description = "Interactor service API documentation";
        });

        return services;
    }
}