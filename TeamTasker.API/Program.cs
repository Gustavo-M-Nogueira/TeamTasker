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


//Configure HTTP request pipeline
app.MapCarter();

app.Run();
