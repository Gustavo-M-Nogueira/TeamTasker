using System.Text;
using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TeamTasker.API.Data;
using TeamTasker.API.Dtos;
using TeamTasker.API.Models;
using TeamTasker.API.Services.Auth.Tokens;
using TeamTasker.API.Services.Auth.Tokens.TokenGenerators;
using TeamTasker.API.Services.Auth.Tokens.TokenValidators;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

// App Services

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
});

// Data Services

builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

// Auth

builder.Services.AddIdentityCore<User>(o =>
{
    o.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization();

AuthConfig authConfig = new AuthConfig();
builder.Configuration.Bind("Authentication", authConfig);
builder.Services.AddSingleton(authConfig);

builder.Services.AddSingleton<TokenGenerator>();
builder.Services.AddSingleton<RefreshTokenGenerator>();
builder.Services.AddSingleton<AccessTokenGenerator>();
builder.Services.AddSingleton<RefreshTokenValidator>();
builder.Services.AddScoped<Authenticator>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.AccessTokenSecret)),
        ValidIssuer = authConfig.Issuer,
        ValidAudience = authConfig.Audience,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ClockSkew = TimeSpan.Zero
    };
});


var app = builder.Build();

//using var scope = app.Services.CreateScope();
//var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//context.Database.MigrateAsync().GetAwaiter().GetResult();

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

app.Run();
