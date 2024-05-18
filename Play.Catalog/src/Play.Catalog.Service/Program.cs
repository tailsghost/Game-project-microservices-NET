
using Play.Common.MongoDb;
using Play.Catalog.Service.Data;
using Play.Common.MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddMongo()
    .AddMongoRepository<Item>("items")
    .AddMassTransitWithRabbitMq();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(build=>
{
    build.WithOrigins(builder.Configuration["AllowedOrigin"])
           .AllowAnyHeader()
           .AllowAnyMethod();
});

app.MapControllers();

app.Run();

