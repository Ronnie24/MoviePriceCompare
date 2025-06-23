using MovieCompareApi.Clients;
using MovieCompareApi.Models;

namespace MovieCompareApi.Services
{
    public class MovieService
    {
        private readonly MovieApiClient _apiClient;
        private readonly ILogger<MovieService> _logger;

        // Constructor injection of the API client and logger
        public MovieService(MovieApiClient apiClient, ILogger<MovieService> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        // Main method that returns the list of movies with the lowest prices between providers
        public async Task<List<MoviePriceComparison>> GetLowestPriceMoviesAsync()
        {
            var result = new List<MoviePriceComparison>();

            // Get movie summaries from both providers
            var cineMovies = await _apiClient.GetMoviesAsync("cinemaworld");
            var filmMovies = await _apiClient.GetMoviesAsync("filmworld");

            //Collect all unique movie titles from both providers
            var allTitles = new HashSet<string>();
            if (cineMovies != null)
                allTitles.UnionWith(cineMovies.Select(m => m.Title));
            if (filmMovies != null)
                allTitles.UnionWith(filmMovies.Select(m => m.Title));

            foreach (var title in allTitles)
            {
                try
                {
                    // Find the ID of the movie from the two providers
                    var cineId = cineMovies?.FirstOrDefault(m => m.Title == title)?.Id;
                    var filmId = filmMovies?.FirstOrDefault(m => m.Title == title)?.Id;

                    MovieDetail? cineDetail = null;
                    MovieDetail? filmDetail = null;

                    if (!string.IsNullOrEmpty(cineId))
                        cineDetail = await _apiClient.GetMovieDetailAsync("cinemaworld", cineId);

                    if (!string.IsNullOrEmpty(filmId))
                        filmDetail = await _apiClient.GetMovieDetailAsync("filmworld", filmId);

                    // Compare prices
                    var prices = new List<(string Provider, decimal Price)>();

                    if (cineDetail?.Price != null)
                        prices.Add(("Cinemaworld", cineDetail.Price.Value));
                    if (filmDetail?.Price != null)
                        prices.Add(("Filmworld", filmDetail.Price.Value));

                    // If any price is available, choose the lowest and add to result
                    if (prices.Any())
                    {
                        var lowest = prices.OrderBy(p => p.Price).First();

                        result.Add(new MoviePriceComparison
                        {
                            Title = title,
                            LowestPrice = lowest.Price,
                            Provider = lowest.Provider
                        });
                    }
                }
                catch (Exception ex)
                {
                    // Log the error for this specific movie, continue with others
                    _logger.LogError($"Error processing movie '{title}': {ex.Message}");
                }
            }

            return result;
        }
    }
}
