namespace StockPriceChangeConsumer.Models
{
    public class StockPriceChange
    {
        public string Ticker { get; set; }
        public decimal PriceChangePercent { get; set; }
    }
}