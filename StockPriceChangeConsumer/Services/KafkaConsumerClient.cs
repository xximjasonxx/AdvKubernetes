
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace StockPriceChangeConsumer.Services
{
    public class KafkaConsumeClient : IConsumerClient
    {
        private IConfiguration _configuration;
        public event EventHandler<string> MessageReceived;

        public KafkaConsumeClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void StartConsuming(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration.GetValue<string>("KafkaHost"),
                GroupId = "price-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe("price-updates");

                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        if (MessageReceived != null)
                            MessageReceived(this, consumeResult.Message.Value);
                        
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
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