using System.Text.Json.Serialization;

namespace MovieCompareApi.Models
{
    public class MovieSummary
    {
        [JsonPropertyName("ID")]
        public string Id { get; set; }

        [JsonPropertyName("Title")]
        public string Title { get; set; }

        [JsonPropertyName("Year")]
        public string Year { get; set; }

        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("Poster")]
        public string Poster { get; set; }
    }
}
