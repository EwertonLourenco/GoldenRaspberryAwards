using GoldenRaspberryAwards.Api.Domain.DTOs;
using GoldenRaspberryAwards.Api.Domain.Entities;
using GoldenRaspberryAwards.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace GoldenRaspberryAwards.Tests.Utilities
{
    public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public Action<AppDbContext> SeedTestData { get; set; } = context => { };

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                services.Remove(descriptor);

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestAwardsDb");
                });

                // Seed de dados
                using var scope = services.BuildServiceProvider().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.EnsureDeleted(); // Limpa o banco para evitar dados residuais
                context.Database.EnsureCreated();

                SeedTestData(context); // Aplica o seed dinâmico
            });
        }
    }
}
