using Book.Archieve.API.Filter;
using Book.Archieve.Application.Definition;
using Book.Archieve.Application.Registration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Is(Serilog.Events.LogEventLevel.Information) 
    .WriteTo.Console() 
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) 
    .CreateLogger();

builder.Host.UseSerilog();

JWTDefinition.Key = builder.Configuration.GetValue<string>("Jwt:Key")!;
JWTDefinition.Issuer = builder.Configuration.GetValue<string>("Jwt:Issuer")!;
string connString = builder.Configuration.GetConnectionString("sqlConnection")!;

builder.Services.RegisterBookArchieve(connString);
builder.Services.AddScoped<ExceptionLoggerFilter>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
