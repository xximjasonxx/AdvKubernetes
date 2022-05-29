using System;
namespace StockDataWorker.Models
{
	public class StockPriceUpdate
	{
		public string Ticker { get; set; }
		public decimal Price { get; set; }

		public StockPriceUpdate()
		{
		}
	}
}

