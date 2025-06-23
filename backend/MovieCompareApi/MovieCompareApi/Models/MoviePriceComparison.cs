namespace MovieCompareApi.Models
{
    public class MoviePriceComparison
    {
        public string Title { get; set; }
        public decimal LowestPrice { get; set; }
        public string Provider { get; set; } // Cinemaworld or Filmworld
    }
}
