using ProfileAnalyzer.Application.Services;
using ProfileAnalyzer.Domain.Interfaces;
using ProfileAnalyzer.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
        }
    );
});

builder.Services.AddControllers();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IFileReader, FileReader>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
