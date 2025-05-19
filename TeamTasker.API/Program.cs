using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
