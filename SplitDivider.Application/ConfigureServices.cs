using System.Reflection;
using AutoMapper.Internal;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SplitDivider.Application.Common.Behaviours;
using SplitDivider.Application.Splits.Graph;
using SplitDivider.Application.Splits.Graph.Interfaces;
using SplitDivider.Application.Users.Commands.CreateUser;

namespace SplitDivider.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IGraphBuilder, GraphBuilder>();
        services.AddScoped<IGraphCutter, MinCutGraphCutter>();
        
        services.AddAutoMapper(cfg => cfg.Internal().MethodMappingEnabled = false, Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Singleton);
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });

        return services;
    }
}
