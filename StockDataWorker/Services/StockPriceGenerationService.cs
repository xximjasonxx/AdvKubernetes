using System;
namespace StockDataWorker.Services
{
	public class StockPriceGenerationService
	{
		private readonly IConfiguration _configuration;

		public StockPriceGenerationService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public decimal GeneratePrice()
        {
			var random = new Random();
			var result = random.Next(
				_configuration.GetValue<int>("PriceMin"),
				_configuration.GetValue<int>("PriceMax")
			);

			// convert to a decimal
			return (decimal)result / 100;
        }
	}
}

