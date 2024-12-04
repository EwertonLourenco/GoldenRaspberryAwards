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
            var winners = GetWinners();

            var producerWins = GroupWinnersByProducer(winners);

            var producerIntervals = CalculateProducerIntervals(producerWins);

            return GetMinMaxIntervals(producerIntervals);
        }

        private List<Movie> GetWinners()
        {
            return _context.Movies
                .Where(m => m.IsWinner)
                .ToList();
        }

        private Dictionary<string, List<int>> GroupWinnersByProducer(List<Movie> winners)
        {
            return winners
                .SelectMany(m => m.Producers.Split(", ").Select(producer => new { producer, year = m.Year }))
                .GroupBy(x => x.producer)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(x => x.year).OrderBy(year => year).ToList()
                );
        }

        private List<ProducerInterval> CalculateProducerIntervals(Dictionary<string, List<int>> producerWins)
        {
            var intervals = new List<ProducerInterval>();

            foreach (var producer in producerWins)
            {
                var years = producer.Value;

                if (years.Count < 2) continue;

                var producerIntervals = years.Zip(years.Skip(1), (prev, next) => new Interval
                {
                    IntervalValue = next - prev,
                    PreviousWin = prev,
                    FollowingWin = next
                }).ToList();

                intervals.Add(new ProducerInterval
                {
                    Producer = producer.Key,
                    Intervals = producerIntervals
                });
            }

            return intervals;
        }

        private AwardIntervalsDto GetMinMaxIntervals(List<ProducerInterval> producerIntervals)
        {
            var minIntervalValue = producerIntervals
                .SelectMany(p => p.Intervals)
                .Min(i => i.IntervalValue);

            var maxIntervalValue = producerIntervals
                .SelectMany(p => p.Intervals)
                .Max(i => i.IntervalValue);

            return new AwardIntervalsDto
            {
                Min = producerIntervals
                    .SelectMany(p => p.Intervals
                        .Where(i => i.IntervalValue == minIntervalValue)
                        .Select(i => new ProducerAwardDto
                        {
                            Producer = p.Producer,
                            Interval = i.IntervalValue,
                            PreviousWin = i.PreviousWin,
                            FollowingWin = i.FollowingWin
                        })
                    )
                    .ToList(),
                Max = producerIntervals
                    .SelectMany(p => p.Intervals
                        .Where(i => i.IntervalValue == maxIntervalValue)
                        .Select(i => new ProducerAwardDto
                        {
                            Producer = p.Producer,
                            Interval = i.IntervalValue,
                            PreviousWin = i.PreviousWin,
                            FollowingWin = i.FollowingWin
                        })
                    )
                    .ToList()
            };
        }


        public List<Movie> GetAllMovies()
        {
            var movies = _context.Movies.ToList();
            return movies;
        }

    }
}
