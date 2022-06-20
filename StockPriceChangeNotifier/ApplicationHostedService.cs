using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockPriceChangeNotifier.Models;
using StockPriceChangeNotifier.Services;

namespace StockPriceChangeNotifier
{
    public class ApplicationHostedService : IHostedService
    {
        private readonly IConsumerClient _consumerClient;
        private readonly ILogger<ApplicationHostedService> _logger;
        private readonly SendNotificationCheckService _sendNotificationCheckService;

        public ApplicationHostedService(IConsumerClient consumerClient, ILogger<ApplicationHostedService> logger,
            SendNotificationCheckService sendNotificationCheckService)
        {
            _consumerClient = consumerClient;
            _logger = logger;
            _sendNotificationCheckService = sendNotificationCheckService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ApplicationHostedService is starting.");
            _consumerClient.MessageReceived += OnMessageReceived;
            _consumerClient.StartConsuming(cancellationToken);
            return Task.CompletedTask;
        }

        private async void OnMessageReceived(object? sender, string e)
        {
            var priceChange = JsonConvert.DeserializeObject<StockPriceChange>(e);
            if (priceChange == null)
            {
                _logger.LogError("Unable to deserialize message: {Message}", e);
                return;
            }

            if (await _sendNotificationCheckService.ShouldSendNotification(priceChange.Ticker, priceChange.PercentChange))
            {
                _logger.LogInformation("Sending notification for {Ticker}", priceChange.Ticker);
                _logger.LogInformation("{PercentChange}% change for {Ticker}", priceChange.PercentChange*100, priceChange.Ticker);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ApplicationHostedService is stopping.");
            _consumerClient.MessageReceived -= OnMessageReceived;
            return Task.CompletedTask;
        }
    }
}