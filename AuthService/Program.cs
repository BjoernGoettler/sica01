using AuthService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(new JwtTokenService());
var app = builder.Build();


app.Run();