using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UserService;
using UserService.Models;
using static UserService.MessageClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<MessageHandler>();
builder.Services.AddSingleton(new MessageClient(RabbitHutch.CreateBus(connectionString:"host=rabbitmq;port=5672;virtualHost=/;username=guest;password=guest")));
builder.Services.AddControllers();
builder.Services.AddDbContext<UserContext>(opt=>opt.UseInMemoryDatabase("Users"));

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
app.UseHttpsRedirection();

app.Run();
Log.CloseAndFlush();
