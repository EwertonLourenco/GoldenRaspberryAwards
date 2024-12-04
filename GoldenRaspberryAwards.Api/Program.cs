using GoldenRaspberryAwards.Api.Application.Services;
using GoldenRaspberryAwards.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("GoldenRaspberryAwards"));

builder.Services.AddScoped<CsvLoaderService>();
builder.Services.AddScoped<AwardsService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var csvLoader = services.GetRequiredService<CsvLoaderService>();

    csvLoader.LoadMovies("movielist.csv");
}

app.Run();

public partial class Program { }
