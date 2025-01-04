using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions
{
    public static class CarterExtension
    {
        public static IServiceCollection AddCarterWithAssembies(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddCarter(configurator: cfg =>
            {
                foreach (var assembly in assemblies)
                {
                    var modules = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();
                    cfg.WithModules(modules);
                }
            });

            return services;
        }
    }
}