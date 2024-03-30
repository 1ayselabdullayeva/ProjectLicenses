﻿using Api.Pipeline;
using Application;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using System.Security.Claims;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
// Configure NLog


//Add NLog as the logger provider

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Information);
});
builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();
builder.Services.AddControllers().AddFluentValidation(/*c=>c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly())*/);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddBusinessLogic(builder.Configuration);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;

    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]);
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
})
.AddJwtBearer("Refresh", options =>
{
    options.RequireHttpsMetadata = false;
    var refreshKey = Encoding.UTF8.GetBytes(builder.Configuration["JWT1:RefreshKey"]);
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT1:Issuer"],
        ValidAudience = builder.Configuration["JWT1:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(refreshKey),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin")); 
    options.AddPolicy("Customer", policy => policy.RequireClaim(ClaimTypes.Role, "Customer"));
});

var app = builder.Build();
//app.UseGlobalErrorHandlingMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

app.UseHttpsRedirection();    app.UseSwaggerUI();
}
app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
