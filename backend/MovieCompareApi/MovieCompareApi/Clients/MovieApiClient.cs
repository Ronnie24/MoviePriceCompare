using MovieCompareApi.Models;
using System.Text.Json;

namespace MovieCompareApi.Clients
{
    public class MovieApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MovieApiClient> _logger;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public MovieApiClient(HttpClient httpClient, IConfiguration config, ILogger<MovieApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiKey = config["MovieApi:ApiKey"];
            _baseUrl = config["MovieApi:BaseUrl"];
        }

        private void AddHeaders()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("x-access-token", _apiKey);
        }

        public async Task<List<MovieSummary>?> GetMoviesAsync(string provider)
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                AddHeaders();
                var response = await _httpClient.GetAsync($"{_baseUrl}/{provider}/movies");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Raw JSON from {Provider}:\n{Json}", provider, content);
                _logger.LogInformation("Movie detail JSON: {Json}", content);
                using var jsonDoc = JsonDocument.Parse(content);
                //var movies = jsonDoc.RootElement.GetProperty("Movies").Deserialize<List<MovieSummary>>();
                //return movies;
                if (jsonDoc.RootElement.TryGetProperty("Movies", out var moviesProp))
                {
                    return moviesProp.Deserialize<List<MovieSummary>>();
                }
                return null;
            });
        }
        public async Task<MovieDetail?> GetMovieDetailAsync(string provider, string movieId)
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                AddHeaders();
                var response = await _httpClient.GetAsync($"{_baseUrl}/{provider}/movie/{movieId}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var detail = JsonSerializer.Deserialize<MovieDetail>(content);
                return detail;
            });
        }

        // Retry helper (最多重试 3 次)
        private async Task<T?> ExecuteWithRetryAsync<T>(Func<Task<T>> action, int maxAttempts = 3)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    return await action();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Attempt {attempt} failed: {ex.Message}");

                    if (attempt == maxAttempts)
                    {
                        _logger.LogError($"All {maxAttempts} attempts failed.");
                        return default;
                    }

                    await Task.Delay(1000 * attempt); // 指数退避策略
                }
            }

            return default;
        }
    }
}
