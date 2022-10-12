using Microsoft.EntityFrameworkCore;
using ProductGrpc.CrossCutting.DI;
using ProductGrpc.Data.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddDbContext<ProductContext>();

Repositories.MakeInjectDependencies(builder.Services);
Services.MakeInjectDependencies(builder.Services);

var app = builder.Build();
GrpcServices.MapGrpcServices(app);

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
