using Microsoft.EntityFrameworkCore;
using ProductGrpc.Application;
using ProductGrpc.Application.Includes;
using ProductGrpc.Data.Context;
using ProductGrpc.Infra.Repository;
using ProductGrpc.Infra.Repository.Includes;
using ProductGrpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddDbContext<ProductContext>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductApplication, ProductApplication>();

var app = builder.Build();

app.MapGrpcService<ProductCreatorService>();
app.MapGet("/", () => "Product Microservice");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapGrpcReflectionService();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ProductContext>();
    context.Database.Migrate();
}


app.Run();
