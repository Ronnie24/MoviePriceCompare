using MovieCompareApi.Clients;
using MovieCompareApi.Models;

namespace MovieCompareApi.Services
{
    public class MovieService
    {
        private readonly MovieApiClient _apiClient;
        private readonly ILogger<MovieService> _logger;

        public MovieService(MovieApiClient apiClient, ILogger<MovieService> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<List<MoviePriceComparison>> GetLowestPriceMoviesAsync()
        {
            var result = new List<MoviePriceComparison>();

            var cineMovies = await _apiClient.GetMoviesAsync("cinemaworld");
            var filmMovies = await _apiClient.GetMoviesAsync("filmworld");

            // 合并两个来源的电影，去重（以 Title 为主）
            var allTitles = new HashSet<string>();
            if (cineMovies != null)
                allTitles.UnionWith(cineMovies.Select(m => m.Title));
            if (filmMovies != null)
                allTitles.UnionWith(filmMovies.Select(m => m.Title));

            foreach (var title in allTitles)
            {
                try
                {
                    // 从两个提供商中查找该电影的 ID
                    var cineId = cineMovies?.FirstOrDefault(m => m.Title == title)?.Id;
                    var filmId = filmMovies?.FirstOrDefault(m => m.Title == title)?.Id;

                    MovieDetail? cineDetail = null;
                    MovieDetail? filmDetail = null;

                    if (!string.IsNullOrEmpty(cineId))
                        cineDetail = await _apiClient.GetMovieDetailAsync("cinemaworld", cineId);

                    if (!string.IsNullOrEmpty(filmId))
                        filmDetail = await _apiClient.GetMovieDetailAsync("filmworld", filmId);

                    // 比较价格
                    var prices = new List<(string Provider, decimal Price)>();

                    if (cineDetail?.Price != null)
                        prices.Add(("Cinemaworld", cineDetail.Price.Value));
                    if (filmDetail?.Price != null)
                        prices.Add(("Filmworld", filmDetail.Price.Value));

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
                    _logger.LogError($"Error processing movie '{title}': {ex.Message}");
                }
            }

            return result;
        }
    }
}
