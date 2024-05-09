
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using Play.Catalog.Service.Repositories;
using MongoDB.Driver;
using Play.Catalog.Service.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

builder.Services.AddSwaggerGen();

builder.Services.AddTransient(opt =>
{
    var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("MongoDbConnection"));
    return mongoClient.GetDatabase(builder.Configuration["ServiceName"]);
});

builder.Services.AddTransient<IRepository<Item>>(service =>
{
    var database = service.GetService<IMongoDatabase>();
    return new MongoRepository<Item>(database, "items");
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

