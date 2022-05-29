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
			var tasks = priceUpdates.Select(x => _producerClient.Send(x, "price-updates"));
			await Task.WhenAll(tasks);

			var failedTasks = tasks.Where(x => x.Status == TaskStatus.Faulted)
				.ToList();

			Console.WriteLine($"{failedTasks.Count} task(s) failed");
        }
    }

	public interface IDataSendService
    {
		Task SendStockPriceData(IList<StockPriceUpdate> priceUpdates);
    }
}

