using Host.Api;
using Modules.Catalog;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonContext.Default));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCatalogModule(builder.Configuration);

builder.Services.AddRequestTimeouts();

builder.Services.AddOutputCache(o =>
    o.AddPolicy("products", p => p.Expire(TimeSpan.FromMinutes(5)).Tag("products")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

//using (var scope = app.Services.CreateScope())
//{
//    try
//    {
//        var dispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatcher>();
//        await dispatcher.SendAsync(new GetAllProductQuery(), CancellationToken.None);
//    }
//    catch (Exception ex)
//    {
//        app.Logger.LogWarning(ex, "Falha ao aquecer o cache de produtos.");
//    }
//}

app.MapGet("/ping", () => "ok");

app.UseAuthentication();
app.UseAuthorization();
app.UseRequestTimeouts();
app.UseOutputCache();

app.MapControllers();
app.MapCatalogModule();

app.Run();