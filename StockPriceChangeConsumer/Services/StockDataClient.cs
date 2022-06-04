using System;
using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using StockPriceChangeConsumer.Entities;

namespace StockPriceChangeConsumer.Services
{
	public class CosmosStocksClient : IClient
	{
		private readonly CosmosClient _cosmosClient;
		private readonly Container _cosmosContainer;

		public CosmosStocksClient(IConfiguration configuration)
		{
			var cosmosDbEndpoint = configuration.GetValue<string>("CosmosEndpointUri");
			var cosmosDbKey = configuration.GetValue<string>("Accesskey");
			var cosmosDbDatabaseName = configuration.GetValue<string>("CosmosDatabaseName");
			var cosmosDbContainerName = configuration.GetValue<string>("CosmosContainerName");

			_cosmosClient = new CosmosClient(cosmosDbEndpoint, cosmosDbKey);
			var cosmosDatabase = _cosmosClient.GetDatabase(cosmosDbDatabaseName);

			_cosmosContainer = cosmosDatabase.GetContainer(cosmosDbContainerName);
		}

        public async Task CreateStockPrice(StockPrice stockPrice)
        {
            await _cosmosContainer.CreateItemAsync(stockPrice, new PartitionKey(stockPrice.Ticker));
        }

        public async Task<StockPrice?> ReadStockPrice(string ticker)
        {
            try
            {
                var result = await _cosmosContainer.ReadItemAsync<StockPrice>(ticker, new PartitionKey(ticker));
                return result.Resource;
            }
            catch (CosmosException cex)
            {
                if (cex.StatusCode == HttpStatusCode.NotFound)
                    return null;

                throw;
            }
        }
    }

	public interface IClient
    {
        Task<StockPrice?> ReadStockPrice(string ticker);

        Task CreateStockPrice(StockPrice stockPrice);
    }
}

