namespace StockPriceChangeConsumer.Entities
{
    public class StockPrice
    {
        public string Ticker { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}