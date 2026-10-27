using Microsoft.EntityFrameworkCore;
using Modules.Catalog;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<Modules.Catalog.Data.CatalogDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("teste")));

builder.Services.AddScoped<
    Modules.Catalog.ICommands.ICommands.ICommandHandler<
        Modules.Catalog.Features.Products.CreateProduct.CreateProductCommand, Guid>,
    Modules.Catalog.Features.Products.CreateProduct.CreateProductCommandHandler>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapCatalogModule();

app.Run();
