using System.Text;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TeamTasker.API.Data;
using TeamTasker.API.Models.DTOs;
using TeamTasker.API.Models.Entities;
using TeamTasker.API.Services.Auth.Authorization.Requirements;
using TeamTasker.API.Services.Auth.Authorization.Services;
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
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);


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

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();
builder.Services.AddScoped<IAuthorizationHandler, TeamMemberRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, TeamLeaderRequirementHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TeamMember", policy => policy.Requirements.Add(new TeamMemberRequirement()));
    options.AddPolicy("TeamLeader", policy => policy.Requirements.Add(new TeamLeaderRequirement()));    
    //options.AddPolicy("TaskCreator", policy => policy.Requirements.Add(new TaskCreatorRequirement()));
    //options.AddPolicy("TaskAssignee", policy => policy.Requirements.Add(new TaskAssigneeRequirement()));
});

// Exceptions
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();
app.UseExceptionHandler(options => { });

app.Run();
