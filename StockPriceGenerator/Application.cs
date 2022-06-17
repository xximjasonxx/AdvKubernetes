using Microsoft.Extensions.Logging;
using StockPriceGenerator.Models;
using StockPriceGenerator.Services;

namespace StockPriceGenerator
{
    public class Application
    {
        private readonly ILogger<Application> _logger;
        private readonly IStockPicksReadService _stockPicksReadService;
        private readonly PriceGenerationService _priceGenerationService;
        private readonly IProducerClient _producerClient;
        
        public Application(ILogger<Application> logger, IStockPicksReadService stockPicksReadService,
            PriceGenerationService priceGenerationService, IProducerClient producerClient)
        {
            _logger = logger;
            _stockPicksReadService = stockPicksReadService;
            _priceGenerationService = priceGenerationService;
            _producerClient = producerClient;
        }

        public async Task Execute()
        {
            var tickers = await _stockPicksReadService.ReadPickedTickers();
            foreach (var ticker in tickers)
            {
                var priceUpdate = new StockPriceUpdate
                {
                    Ticker = ticker,
                    Price = _priceGenerationService.GeneratePrice()
                };

                _logger.LogInformation($"Sending update: {priceUpdate.ToString()}");
                await _producerClient.Send(priceUpdate, "stock-price-updates");
            }
        }
    }
}