using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using TweetService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TweetContext>(
    opt => opt.UseInMemoryDatabase("Tweets"));

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
