using Microsoft.AspNetCore.Mvc;
using MovieCompareApi.Models;
using MovieCompareApi.Services;

namespace MovieCompareApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly MovieService _movieService;
        private readonly ILogger<MovieController> _logger;

        public MovieController(MovieService movieService, ILogger<MovieController> logger)
        {
            _movieService = movieService;
            _logger = logger;
        }

        [HttpGet("compare-prices")]
        public async Task<ActionResult<List<MoviePriceComparison>>> GetCheapestMovies()
        {
            try
            {
                var result = await _movieService.GetLowestPriceMoviesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get movie comparisons: {ex.Message}");
                return StatusCode(500, "Internal server error while comparing movie prices.");
            }
        }
    }
}
