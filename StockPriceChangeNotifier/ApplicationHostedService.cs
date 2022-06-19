using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StockPriceChangeNotifier
{
    public class ApplicationHostedService : IHostedService
    {
        private readonly IConsumerClient _consumerClient;
        private readonly ILogger<ApplicationHostedService> _logger;

        public ApplicationHostedService(IConsumerClient consumerClient, ILogger<ApplicationHostedService> logger)
        {
            _consumerClient = consumerClient;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ApplicationHostedService is starting.");
            _consumerClient.MessageReceived += OnMessageReceived;
            _consumerClient.StartConsuming(cancellationToken);
            return Task.CompletedTask;
        }

        private void OnMessageReceived(object? sender, string e)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ApplicationHostedService is stopping.");
            _consumerClient.MessageReceived -= OnMessageReceived;
            return Task.CompletedTask;
        }
    }
}