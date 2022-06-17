using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace StockPriceGenerator
{
    public class KafkaProducerClient : IProducerClient
	{
		private readonly IConfiguration _configuration;

		private IProducer<Null, string> _producerClient;
		private IProducer<Null, string> ProducerClient
        {
			get
            {
				if (_producerClient == null)
                {
					var producerConfig = new ProducerConfig
					{
						BootstrapServers = _configuration.GetValue<string>("KafkaHost")
					};

					_producerClient = new ProducerBuilder<Null, string>(producerConfig).Build();
                }

				return _producerClient;
            }
        }

		public KafkaProducerClient(IConfiguration configuration)
		{
			_configuration = configuration;
		}

        public async Task Send<T>(T sendObject, string topicName) where T : class
        {
			try
			{
				// serialize the incoming object
				var objectString = JsonConvert.SerializeObject(sendObject);

				// send the message
				await ProducerClient.ProduceAsync(
					topicName,
					new Message<Null, string> { Value = objectString }
				);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
        }
    }

	public interface IProducerClient
    {
		Task Send<T>(T sendObject, string topicName) where T : class;
    }
}