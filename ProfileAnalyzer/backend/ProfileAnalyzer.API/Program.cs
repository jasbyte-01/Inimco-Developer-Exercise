using Microsoft.AspNetCore.Mvc;
using ProfileAnalyzer.Application.Services;
using ProfileAnalyzer.Domain.Interfaces;
using ProfileAnalyzer.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAngularApp",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200") // Angular app URL
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register dependencies
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IFileReader, FileReader>();
builder.Services.AddScoped<INameAnalysisService, NameAnalysisService>();
builder.Services.AddScoped<IUserService, UserService>();

// Configure JSON formatting to match requirements
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Maintain PascalCase
});

var app = builder.Build();

// Configure the HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
