using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using StockPriceChangeConsumer.Entities;
using StockPriceChangeConsumer.Models;
using StockPriceChangeConsumer.Services;

namespace StockPriceChangeConsumer
{
    public class ApplicationHostedService : IHostedService
    {
        private readonly IConsumerClient _consumerClient;
        private readonly IClient _stocksClient;
        private readonly StockPriceChangeCalculationService _stockPriceChangeCalculationService;

        public ApplicationHostedService(IConsumerClient consumerClient,
            IClient stocksClient,
            StockPriceChangeCalculationService stockPriceChangeCalculationService)
        {
            _consumerClient = consumerClient;
            _stocksClient = stocksClient;
            _stockPriceChangeCalculationService = stockPriceChangeCalculationService;

            _consumerClient.MessageReceived += OnMessageReceived;
        }

        private async void OnMessageReceived(object? sender, string objectString)
        {
            var priceUpdate = JsonConvert.DeserializeObject<StockPriceUpdate>(objectString);
            if (priceUpdate == null)
                throw new Exception("The received message did not serialize into a StockPriceUpdate object.");

            // create the new result
            var newStockPrice = new StockPrice
            {
                Ticker = priceUpdate.Ticker,
                Price = priceUpdate.Price,
                Time = DateTimeOffset.UtcNow
            };

            // read the old stock price
            var oldStockPrice = await _stocksClient.ReadStockPrice(priceUpdate.Ticker);
            if (oldStockPrice == null)
            {
                // create the stock price
                await _stocksClient.CreateStockPrice(newStockPrice);
            }
            else
            {
                // calculate the change
                Console.WriteLine("Old Stock Price Exists - need to calculate percent change");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumerClient.StartConsuming(cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}