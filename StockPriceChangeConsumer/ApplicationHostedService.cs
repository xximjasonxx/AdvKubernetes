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
        private readonly IStockPriceLookupService _stockPriceLookupService;

        public ApplicationHostedService(IConsumerClient consumerClient,
            IStockPriceLookupService stockPriceLookupService)
        {
            _consumerClient = consumerClient;
            _stockPriceLookupService = stockPriceLookupService;

            _consumerClient.MessageReceived += OnMessageReceived;
        }

        private async void OnMessageReceived(object? sender, string objectString)
        {
            var priceUpdate = JsonConvert.DeserializeObject<StockPriceUpdate>(objectString);
            if (priceUpdate == null)
                throw new Exception("The received message did not serialize into a StockPriceUpdate object.");

            // create our new stock price change object
            var newStockPrice = new StockPrice
            {
                Ticker = priceUpdate.Ticker,
                Price = priceUpdate.Price,
                Time = DateTimeOffset.UtcNow
            };

            var currentStockPrice = await _stockPriceLookupService.GetStockPrice(priceUpdate.Ticker);
            if (currentStockPrice != null)
            {
                // prepare to send a percent change event
            }

            // save the new stock price
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