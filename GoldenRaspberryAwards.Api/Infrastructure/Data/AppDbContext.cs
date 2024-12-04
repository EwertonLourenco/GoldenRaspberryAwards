using Microsoft.EntityFrameworkCore;
using GoldenRaspberryAwards.Api.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace GoldenRaspberryAwards.Api.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().HasKey(m => m.Id);
        }
    }
}
