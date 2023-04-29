using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using Shared.Values.EventBus.InteractionEvents;
using Shared.Values.EventBus.UserEvents;
using SplitDivider.Application.Common.Interfaces;
using SplitDivider.WebApp.EventBus;
using SplitDivider.WebApp.EventBus.Handlers.InteractionEvents;
using SplitDivider.WebApp.EventBus.Handlers.UserEvents;
using SplitDivider.WebApp.Services;
using ZymLabs.NSwag.FluentValidation;

namespace SplitDivider.WebApp;

public static class ConfigureServices
{
    public static IServiceCollection AddWebAppServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks();
            //.AddDbContextCheck<ApplicationDbContext>();

        services.AddControllersWithViews();
        
        services.AddScoped<FluentValidationSchemaProcessor>(provider =>
        {
            var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
            var loggerFactory = provider.GetService<ILoggerFactory>();

            return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
        });

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddOpenApiDocument((configure, serviceProvider) =>
        {
            var fluentValidationSchemaProcessor = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<FluentValidationSchemaProcessor>();

            // Add the fluent validations schema processor
            configure.SchemaProcessors.Add(fluentValidationSchemaProcessor);

            configure.Title = "Split Divider API";
            configure.Description = "Split-divider service API documentation";

            // configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            // {
            //     Type = OpenApiSecuritySchemeType.ApiKey,
            //     Name = "Authorization",
            //     In = OpenApiSecurityApiKeyLocation.Header,
            //     Description = "Type into the textbox: Bearer {your JWT token}."
            // });
            //
            // configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });

        return services;
    }

    public static Receiver ConfigureEventSubscriptionReceiver(IServiceProvider serviceProvider)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            DispatchConsumersAsync = true
        };
        
        var receiver = new Receiver(factory);

        receiver.SubscribeToEvent(new UserRegisteredEvent(), new UserRegisteredEventHandler(serviceProvider));
        receiver.SubscribeToEvent(new UserChangedEvent(), new UserChangedEventHandler(serviceProvider));
        receiver.SubscribeToEvent(new UserDeletedEvent(), new UserDeletedEventHandler(serviceProvider));
        receiver.SubscribeToEvent(new UsersInteractedEvent(), new UsersInteractedEventHandler(serviceProvider));

        return receiver;
    }
}
