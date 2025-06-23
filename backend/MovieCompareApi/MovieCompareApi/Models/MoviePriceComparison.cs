namespace MovieCompareApi.Models
{
    public class MoviePriceComparison
    {
        public string Title { get; set; }
        public decimal LowestPrice { get; set; } // The lowest price found among all providers
        public string Provider { get; set; } // Cinemaworld or Filmworld
    }
}
