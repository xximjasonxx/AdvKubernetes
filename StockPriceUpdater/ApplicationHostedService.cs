using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StockPriceUpdater
{
    public class ApplicationHostedService : IHostedService
    {
        private readonly IConsumerClient _consumerClient;
        private readonly ILogger<ApplicationHostedService> _logger;

        public ApplicationHostedService(IConsumerClient consumerClient, ILogger<ApplicationHostedService> logger)
        {
            _consumerClient = consumerClient;
            _logger = logger;

            _consumerClient.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object? sender, string e)
        {
            _logger.LogInformation($"Message received: {e}");
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