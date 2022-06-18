namespace StockPriceUpdater.Models
{
    public class StockPriceChange
    {
        public string Ticker { get; internal set; }
        public decimal PercentChange { get; internal set; }
    }
}