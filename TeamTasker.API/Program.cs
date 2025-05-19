using Carter;
using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

//App Services

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
});

//Data Services

builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Database")));


var app = builder.Build();

//using var scope = app.Services.CreateScope();
//var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//context.Database.MigrateAsync().GetAwaiter().GetResult();

//Configure HTTP request pipeline
app.MapCarter();

app.Run();
