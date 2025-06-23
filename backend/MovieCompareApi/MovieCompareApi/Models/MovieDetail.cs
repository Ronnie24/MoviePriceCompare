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
        [JsonPropertyName("Price")]
        public string PriceRaw { get; set; }  

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
