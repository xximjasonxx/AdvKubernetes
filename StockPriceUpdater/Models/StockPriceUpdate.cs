namespace StockPriceUpdater.Models
{
    public class StockPriceUpdate
    {
        public string Ticker  { get; set; }
        public decimal Price  { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }
}