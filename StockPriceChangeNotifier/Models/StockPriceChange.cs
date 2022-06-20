namespace StockPriceChangeNotifier.Models
{
    public class StockPriceChange
    {
        public string Ticker { get; set; }
        public decimal PercentChange { get; set; }
    }
}