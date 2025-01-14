using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Interceptors;


namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection service, IConfiguration configuration)
    {
        // service.AddMediatR(config =>
        // {
        //     config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        //     config.AddOpenBehavior(typeof(ValidateBehavior<,>));
        //     config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        // });
        // service.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        var connectionString = configuration.GetConnectionString("Database");
        service.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        service.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        service.AddDbContext<CatalogDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetService<ISaveChangesInterceptor>()!);
            options.UseNpgsql(connectionString);
        });
        service.AddScoped<IDataSeeder, CatalogDataSeeder>();
        return service;
    }

    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        app.UseMigration<CatalogDbContext>();
        return app;
    }
}
