using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json", false, false);
builder.Services.AddOcelot(builder.Configuration);
var app = builder.Build();

app.UseOcelot().Wait();

app.Run();