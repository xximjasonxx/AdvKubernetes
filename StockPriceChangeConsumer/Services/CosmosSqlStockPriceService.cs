using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using StockPriceChangeConsumer.Entities;

/*namespace StockPriceChangeConsumer.Services
{
    public class CosmosSqlStockPriceService : IStockPriceService
    {
        

        public CosmosSqlStockPriceService(IConfiguration configuration)
        {
            var cosmosDbEndpoint = configuration.GetValue<string>("CosmosEndpointUri");
            var cosmosDbKey = configuration.GetValue<string>("Accesskey");
            var cosmosDbDatabaseName = configuration.GetValue<string>("CosmosDatabaseName");
            var cosmosDbContainerName = configuration.GetValue<string>("CosmosContainerName");

            _cosmosClient = new CosmosClient(cosmosDbEndpoint, cosmosDbKey);
            _cosmosDatabase = _cosmosClient.GetDatabase(cosmosDbDatabaseName);
            _cosmosContainer = _cosmosDatabase.GetContainer(cosmosDbContainerName);
        }
        
        public async Task<StockPrice?> GetStockPrice(string ticker)
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

        public async Task CreateStockPrice(StockPrice stockPrice)
        {
            await _cosmosContainer.CreateItemAsync(stockPrice, new PartitionKey(stockPrice.Ticker));
        }

        public async Task ReplaceStockPrice(StockPrice newStockPrice)
        {
            await _cosmosContainer.UpsertItemAsync(newStockPrice, new PartitionKey(newStockPrice.Ticker));
        }
    }

    public interface IStockPriceService
    {
        Task<StockPrice?> GetStockPrice(string ticker);
        
        Task CreateStockPrice(StockPrice newStockPrice);
        Task ReplaceStockPrice(StockPrice newStockPrice);
    }
}*/