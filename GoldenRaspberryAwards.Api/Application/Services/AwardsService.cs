using GoldenRaspberryAwards.Api.Domain.Entities;
using GoldenRaspberryAwards.Api.Infrastructure.Data;
using GoldenRaspberryAwards.Api.Domain.DTOs;
using GoldenRaspberryAwards.Api.Application.Models;

namespace GoldenRaspberryAwards.Api.Application.Services
{
    public class AwardsService
    {
        private readonly AppDbContext _context;

        public AwardsService(AppDbContext context)
        {
            _context = context;
        }

        public AwardIntervalsDto GetAwardIntervals()
        {
            var winners = _context.Movies
                .Where(m => m.IsWinner)
                .ToList();

            var producerWins = new Dictionary<string, List<int>>();

            foreach (var movie in winners)
            {
                var producers = movie.Producers
                    .Split(new[] { ",", " e ", " and " }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim());

                foreach (var producer in producers)
                {
                    if (!producerWins.ContainsKey(producer))
                    {
                        producerWins[producer] = new List<int>();
                    }
                    producerWins[producer].Add(movie.Year);
                }
            }

            var producerIntervals = new List<ProducerInterval>();

            foreach (var producer in producerWins)
            {
                var years = producer.Value.OrderBy(y => y).ToList();
                for (int i = 0; i < years.Count - 1; i++)
                {
                    producerIntervals.Add(new ProducerInterval
                    {
                        Producer = producer.Key,
                        Interval = years[i + 1] - years[i],
                        PreviousWin = years[i],
                        FollowingWin = years[i + 1]
                    });
                }
            }

            var minInterval = producerIntervals.Min(pi => pi.Interval);
            var maxInterval = producerIntervals.Max(pi => pi.Interval);

            var minProducers = producerIntervals
                .Where(pi => pi.Interval == minInterval)
                .Select(pi => new ProducerAwardDto
                {
                    Producer = pi.Producer,
                    Interval = pi.Interval,
                    PreviousWin = pi.PreviousWin,
                    FollowingWin = pi.FollowingWin
                })
                .ToList();

            var maxProducers = producerIntervals
                .Where(pi => pi.Interval == maxInterval)
                .Select(pi => new ProducerAwardDto
                {
                    Producer = pi.Producer,
                    Interval = pi.Interval,
                    PreviousWin = pi.PreviousWin,
                    FollowingWin = pi.FollowingWin
                })
                .ToList();

            return new AwardIntervalsDto
            {
                Min = minProducers,
                Max = maxProducers
            };
        }

        public List<Movie> GetAllMovies()
        {
            var movies = _context.Movies.ToList();
            return movies;
        }

    }
}
