using System;
using StockDataWorker.Models;

namespace StockDataWorker.Services
{
	public class KafkaDataSendService : IDataSendService
	{
		private readonly IProducerClient _producerClient;

		public KafkaDataSendService(IProducerClient producerClient)
		{
			_producerClient = producerClient;
		}

        public async Task SendStockPriceData(IList<StockPriceUpdate> priceUpdates)
        {

			foreach (var priceUpdate in priceUpdates)
			{
				await _producerClient.Send(priceUpdate, "price-updates");
				Console.WriteLine($"Sent price update for {priceUpdate.Ticker}");
			}

			Console.WriteLine($"Sent {priceUpdates.Count} price updates");
        }
    }

	public interface IDataSendService
    {
		Task SendStockPriceData(IList<StockPriceUpdate> priceUpdates);
    }
}

