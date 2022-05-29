using System;
using StockDataWorker.Models;

namespace StockDataWorker.Services
{
	public class StockDataGenerationService
	{
		private readonly StockPriceGenerationService _stockPriceGenerationService;

		public StockDataGenerationService(StockPriceGenerationService stockPriceGenerationService)
		{
			_stockPriceGenerationService = stockPriceGenerationService;
		}

		public IEnumerable<StockPriceUpdate> GetStockPrices(IList<StockTicker> tickers)
        {
			foreach (var ticker in tickers)
            {
				yield return new StockPriceUpdate
				{
					Ticker = ticker.Ticker,
					Price = _stockPriceGenerationService.GeneratePrice()
				};
            }
        }
	}
}

