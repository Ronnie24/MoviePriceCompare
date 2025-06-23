using System.Text.Json.Serialization;

namespace MovieCompareApi.Models
{
    public class MovieDetail
    {
        [JsonPropertyName("ID")]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Year { get; set; }
        public string Poster { get; set; }

        // Maps the JSON field "Price" to this string property
        [JsonPropertyName("Price")]
        public string PriceRaw { get; set; }

        // Computed property: safely converts PriceRaw to a decimal (nullable)
        // This property is ignored when serializing to JSON
        [JsonIgnore]
        public decimal? Price
        {
            get
            {
                if (decimal.TryParse(PriceRaw, out var price))
                    return price;
                return null;
            }
        }
    }
}
