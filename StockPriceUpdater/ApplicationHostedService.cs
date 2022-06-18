using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockPriceUpdater.Models.Services;

namespace StockPriceUpdater.Models
{
    public class ApplicationHostedService : IHostedService
    {
        private readonly IConsumerClient _consumerClient;
        private readonly IProducerClient _producerClient;
        private readonly IStockPriceWriteService _stockPriceWriteService;
        private readonly IStockPriceReadService _stockPriceReadService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApplicationHostedService> _logger;

        public ApplicationHostedService(IConsumerClient consumerClient, ILogger<ApplicationHostedService> logger,
            IStockPriceWriteService stockPriceWriteService, IStockPriceReadService stockPriceReadService,
            IProducerClient producerClient, IConfiguration configuration)
        {
            _consumerClient = consumerClient;
            _producerClient = producerClient;
            _stockPriceWriteService = stockPriceWriteService;
            _stockPriceReadService = stockPriceReadService;
            _configuration = configuration;
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
                var percentChange = Math.Round((stockUpdate.Price - oldStock.Price) / oldStock.Price, 4);

                _logger.LogInformation("Sending price change for {Ticker}: {PercentChange}%", stockUpdate.Ticker, percentChange*100);
                await _producerClient.Send(new StockPriceChange
                {
                    Ticker = stockUpdate.Ticker,
                    PercentChange = percentChange
                }, _configuration["PriceChangeTopic"]);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting application hosted service");
            _consumerClient.StartConsuming(cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping application hosted service");
            return Task.CompletedTask;
        }
    }
}