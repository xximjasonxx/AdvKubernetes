
using Confluent.Kafka;

namespace StockPriceChangeConsumer.Services
{
    public class KafkaConsumeClient : IConsumerClient
    {
        public event EventHandler<string> MessageReceived;

        public void StartConsuming(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
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