using Microsoft.Extensions.Configuration;

namespace StockPriceGenerator.Services
{
    public class PriceGenerationService
    {
        private readonly IConfiguration _configuration;

		public PriceGenerationService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public decimal GeneratePrice()
        {
			var random = new Random();
			var resultRaw = random.NextDouble() * 10;

			// convert to a decimal
			return (decimal)Math.Round(resultRaw, 2);
        }
    }
}