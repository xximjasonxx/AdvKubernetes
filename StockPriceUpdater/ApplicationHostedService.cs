using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockPriceUpdater.Models.Services;

namespace StockPriceUpdater.Models
{
    public class ApplicationHostedService : IHostedService
    {
        private readonly IConsumerClient _consumerClient;
        private readonly IStockPriceWriteService _stockPriceWriteService;
        private readonly IStockPriceReadService _stockPriceReadService;
        private readonly ILogger<ApplicationHostedService> _logger;

        public ApplicationHostedService(IConsumerClient consumerClient, ILogger<ApplicationHostedService> logger,
            IStockPriceWriteService stockPriceWriteService, IStockPriceReadService stockPriceReadService)
        {
            _consumerClient = consumerClient;
            _stockPriceWriteService = stockPriceWriteService;
            _stockPriceReadService = stockPriceReadService;
            _logger = logger;

            _consumerClient.MessageReceived += OnMessageReceived;
        }

        private async void OnMessageReceived(object? sender, string e)
        {
            if (string.IsNullOrEmpty(e))
            {
                _logger.LogError("Received empty message");
                return;
            }

            var stockUpdate = JsonConvert.DeserializeObject<StockPriceUpdate>(e);
            if (stockUpdate == null)
            {
                _logger.LogError("Invalid message received: {Message}", e);
                return;
            }

            var oldStock = await _stockPriceReadService.ReadStockPrice(stockUpdate.Ticker);
            
            stockUpdate.UpdatedAt = DateTime.UtcNow;
            await _stockPriceWriteService.WriteStockPrice(stockUpdate);

            if (oldStock != null)
            {
                // calculate the change
                var percentChange = Math.Round((stockUpdate.Price - oldStock.Price) / oldStock.Price, 2);

                _logger.LogInformation("Stock price change for {Ticker}: {PercentChange}%", stockUpdate.Ticker, percentChange*100);
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