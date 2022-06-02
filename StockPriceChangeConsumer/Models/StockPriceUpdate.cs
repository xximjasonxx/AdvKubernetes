namespace StockPriceChangeConsumer.Models
{
    public class StockPriceUpdate
	{
		public string Ticker { get; set; }
		public decimal Price { get; set; }

        public override string ToString()
        {
            return $"{Ticker} {Price}";
        }
	}
}