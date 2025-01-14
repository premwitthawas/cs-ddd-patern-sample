var builder = WebApplication.CreateBuilder(args);
var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));
builder.Services.AddCarterWithAssembies([catalogAssembly, basketAssembly]);
builder.Services.AddMediatRWithAssemblies([catalogAssembly, basketAssembly]);

builder.Services.AddStackExchangeRedisCache(opt=>{
    opt.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddMassTransitWithAssemblies(builder.Configuration,
 [catalogAssembly, basketAssembly]);

builder.Services
.AddCatalogModule(builder.Configuration)
.AddBasketModule(builder.Configuration)
.AddOrderingModule(builder.Configuration);
builder.Services.AddExceptionHandler<CustomExceptionHandle>();
var app = builder.Build();
app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(options => { });
app
.UseCatalogModule()
.UseBasketModule()
.UseOrderingModule();
app.Run();
