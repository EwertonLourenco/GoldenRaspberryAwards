using GoldenRaspberryAwards.Api.Application.Services;
using GoldenRaspberryAwards.Api.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoldenRaspberryAwards.Api.Controllers
{
    [ApiController]
    [Route("api/awards")]
    public class AwardsController : ControllerBase
    {
        private readonly AwardsService _service;

        public AwardsController(AwardsService service)
        {
            _service = service;
        }

        [HttpGet("intervals")]
        public ActionResult<AwardIntervalsDto> GetAwardIntervals()
        {
            var intervals = _service.GetAwardIntervals();
            return Ok(intervals);
        }

        [HttpGet("debug/movies")]
        public IActionResult GetAllMovies()
        {
            var movies = _service.GetAllMovies();
            return Ok(movies);
        }

    }
}
