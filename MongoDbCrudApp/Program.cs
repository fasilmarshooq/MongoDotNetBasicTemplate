using Agoda.IoC.NetCore;
using Microsoft.EntityFrameworkCore;
using MongoDbCrudApp.Data;
using MongoDotNetMigratorLight;

var builder = WebApplication.CreateBuilder(args);


//Auto register all classes that have the RegisterPerRequest attribute , refer : https://github.com/agoda-com/Agoda.IoC
builder.Services.AutoWireAssembly(new[] { typeof(Program).Assembly }, false);

builder.Services.AddDbContext<Repository>(options =>
options.UseMongoDB("mongodb://root:sa@localhost:27017/", "studentDb"));

builder.Services.AddMongoNetMigratorLight("mongodb://root:sa@localhost:27017/", "studentDb", typeof(Program).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.RunMigration().Wait();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
