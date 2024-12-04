using System.Net.Http.Json;
using GoldenRaspberryAwards.Api;
using GoldenRaspberryAwards.Api.Application.Services;
using GoldenRaspberryAwards.Api.Domain.DTOs;
using GoldenRaspberryAwards.Api.Domain.Entities;
using GoldenRaspberryAwards.Api.Infrastructure.Data;
using GoldenRaspberryAwards.Tests.Utilities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace GoldenRaspberryAwards.Tests.Integration
{
    [Collection("Integration Tests")]
    public class AwardsControllerTests : IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AwardsControllerTests(TestWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public void Should_Calculate_Correct_Award_Intervals()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie { Year = 1980, Title = "Movie A", Studios = "Studio A", Producers = "Producer A", IsWinner = true },
                new Movie { Year = 1985, Title = "Movie B", Studios = "Studio B", Producers = "Producer A", IsWinner = true },
                new Movie { Year = 1990, Title = "Movie C", Studios = "Studio C", Producers = "Producer A", IsWinner = true },
                new Movie { Year = 1982, Title = "Movie D", Studios = "Studio D", Producers = "Producer B", IsWinner = true },
                new Movie { Year = 1988, Title = "Movie E", Studios = "Studio E", Producers = "Producer B", IsWinner = true }
            };

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("UnitTestDb")
                .Options;

            using var context = new AppDbContext(options);
            context.Movies.AddRange(movies);
            context.SaveChanges();

            var service = new AwardsService(context);

            // Act
            var result = service.GetAwardIntervals();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Min);
            Assert.NotEmpty(result.Max);

            Assert.True(result.Min.Any(min => min.Interval > 0), "There should be at least one valid Min interval.");
            Assert.True(result.Max.Any(max => max.Interval > 0), "There should be at least one valid Max interval.");
        }


        [Fact]
        public async Task Should_Return_Award_Intervals_From_Endpoint()
        {
            // Act
            var response = await _client.GetAsync("api/awards/intervals");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AwardIntervalsDto>(responseContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Min);
            Assert.NotNull(result.Max);

            Assert.True(result.Min.Any(), "The Min list should not be empty.");
            Assert.True(result.Max.Any(), "The Max list should not be empty.");

            // Ensure valid structure
            foreach (var interval in result.Min.Concat(result.Max))
            {
                Assert.NotNull(interval.Producer);
                Assert.True(interval.Interval > 0, "Interval should be greater than 0.");
                Assert.True(interval.PreviousWin > 0, "PreviousWin should be a valid year.");
                Assert.True(interval.FollowingWin > 0, "FollowingWin should be a valid year.");
                Assert.True(interval.PreviousWin < interval.FollowingWin, "PreviousWin should be before FollowingWin.");
            }
        }
    }
}
