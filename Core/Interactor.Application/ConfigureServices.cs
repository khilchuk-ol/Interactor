using Interactor.Application.Common.Interfaces;
using Interactor.Application.Common.Services;
using Interactor.Application.EventBus;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Interactor.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        services.AddSingleton<IAsyncConnectionFactory>(factory);
        services.AddScoped<IEventBusPublisher, Publisher>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IInteractionService, InteractionService>();

        return services;
    }
}
