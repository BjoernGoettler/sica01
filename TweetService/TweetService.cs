using EasyNetQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using TweetService.Models;
using static TweetService.MessageClient;
using TweetService;
using TweetService.Interfaces;
using TweetService.Repositories;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration.GetSection("Settings").Get<Settings>();

string jwtIssuer = config.JwtIssuer;
string jwtKey = config.JwtKey;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey))
        };
    }
);

builder.Services.AddSingleton(jwtIssuer);
builder.Services.AddSingleton(jwtKey);
builder.Services.AddHostedService<MessageHandler>();
builder.Services.AddSingleton(new MessageClient(RabbitHutch.CreateBus(connectionString:"host=rabbitmq;port=5672;virtualHost=/;username=guest;password=guest")));
builder.Services.AddControllers();
builder.Services.AddDbContext<TweetContext>(
    opt => opt.UseInMemoryDatabase("Tweets"));

builder.Services.AddScoped<ITweetRepository, TweetRepository>();

// Add services to the container.
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
app.MapControllers();
app.UseAuthentication();
app.Run();
Log.CloseAndFlush();
