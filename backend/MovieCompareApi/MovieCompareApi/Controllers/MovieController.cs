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

        // Constructor injection of MovieService and logger
        public MovieController(MovieService movieService, ILogger<MovieController> logger)
        {
            _movieService = movieService;
            _logger = logger;
        }

        // GET endpoint: api/movie/compare-prices
        [HttpGet("compare-prices")]
        public async Task<ActionResult<List<MoviePriceComparison>>> GetCheapestMovies()
        {
            try
            {
                // Calls the service to get the lowest price for each movie
                var result = await _movieService.GetLowestPriceMoviesAsync();
                return Ok(result); // Returns HTTP 200 with the result
            }
            catch (Exception ex)
            {
                // Logs any errors and returns a 500 Internal Server Erro
                _logger.LogError($"Failed to get movie comparisons: {ex.Message}");
                return StatusCode(500, "Internal server error while comparing movie prices.");
            }
        }
    }
}
