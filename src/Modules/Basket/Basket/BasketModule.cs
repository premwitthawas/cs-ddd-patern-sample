using Basket.Basket.Repositories;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Shared.Data;
namespace Basket;

public static class BasketModule
{
    public static IServiceCollection AddBasketModule(this IServiceCollection service, IConfiguration configuraion)
    {
        service.AddScoped<IBasketRepository, BasketRepository>();
        service.Decorate<IBasketRepository, CachedBasketRepository>();
        // service.AddScoped<IBasketRepository, CachedBasketRepository>();
        //manuanlly register the interceptor
        // service.AddScoped<IBasketRepository>(provider=>{
        //     var basketRepository = provider.GetRequiredService<BasketRepository>();
        //     return new CachedBasketRepository(basketRepository,provider.GetRequiredService<IDistributedCache>());
        // });
        var connectionString = configuraion.GetConnectionString("Database");
        service.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        service.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        service.AddDbContext<BasketDbContext>((sp, opt) =>
        {
            opt.AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>());
            opt.UseNpgsql(connectionString);
        });
        return service;
    }
    public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
    {
        app.UseMigration<BasketDbContext>();
        return app;
    }
}
