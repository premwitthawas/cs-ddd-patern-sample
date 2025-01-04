
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarterWithAssembies(typeof(CatalogModule).Assembly);

builder.Services
.AddCatalogModule(builder.Configuration)
.AddBasketModule(builder.Configuration)
.AddOrderingModule(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
var app = builder.Build();
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

app.MapCarter();

app
.UseCatalogModule()
.UseBasketModule()
.UseOrderingModule();
// app.UseHttpsRedirection();
// app.UseStaticFiles();
// app.UseAuthentication();
// app.UseAuthorization();
// app.MapControllers();

app.Run();
