using System.Reflection;
using Application.Common.Behaviours;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    /// <summary>
    /// Extension Class For <see cref="IServiceCollection"/> Interface
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Injects Application Dependencies Into Dependency Injection Container
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> Interface</param>
        /// <param name="configuration"><see cref="IConfiguration"/> Interface</param>
        public static void AddApplication(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSingleton(TypeAdapterConfig.GlobalSettings);
            services.AddScoped<IMapper, ServiceMapper>();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TaskCanceledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        }
    }
}