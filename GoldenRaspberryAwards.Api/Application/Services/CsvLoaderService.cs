using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using GoldenRaspberryAwards.Api.Domain.Entities;
using GoldenRaspberryAwards.Api.Infrastructure.Data;

namespace GoldenRaspberryAwards.Api.Application.Services
{
    public class CsvLoaderService
    {
        private readonly AppDbContext _context;

        public CsvLoaderService(AppDbContext context)
        {
            _context = context;
        }

        public void LoadMovies(string csvPath)
        {
            if (string.IsNullOrEmpty(csvPath))
            {
                csvPath = Path.Combine(AppContext.BaseDirectory, "movielist.csv");
            }

            using var reader = new StreamReader(csvPath);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";", 
                HeaderValidated = null,
                MissingFieldFound = null
            };
            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap<MovieMap>();
            var movies = csv.GetRecords<Movie>().ToList();

            _context.Movies.AddRange(movies);
            _context.SaveChanges();
        }
    }

    public sealed class MovieMap : ClassMap<Movie>
    {
        public MovieMap()
        {
            Map(m => m.Year).Name("year");
            Map(m => m.Title).Name("title");
            Map(m => m.Studios).Name("studios");
            Map(m => m.Producers).Name("producers");
            Map(m => m.IsWinner).Name("winner").Convert(row => row.Row.GetField("winner") == "yes");
        }
    }
}
