using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace StockPriceUpdater
{
    public class KafkaConsumeClient : IConsumerClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<KafkaConsumeClient> _logger;
        public event EventHandler<string> MessageReceived;

        public KafkaConsumeClient(IConfiguration configuration, ILogger<KafkaConsumeClient> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void StartConsuming(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["KafkaHost"],
                GroupId = _configuration["ConsumerGroup"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(_configuration["KafkaTopic"]);

                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        if (MessageReceived != null)
                        {
                            _logger.LogInformation($"Message received: {consumeResult.Message.Value}");
                            MessageReceived(this, consumeResult.Message.Value);
                        }
                        
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Error occured: {e.Error.Reason}");
                    }
                }
            }
        }
    }

    public interface IConsumerClient
    {
        event EventHandler<string> MessageReceived;

        void StartConsuming(CancellationToken cancellationToken);
    }
}