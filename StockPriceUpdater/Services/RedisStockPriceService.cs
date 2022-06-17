using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace StockPriceUpdater.Models.Services
{
    public class RedisStockPriceService : IStockPriceWriteService, IStockPriceReadService
    {
        private readonly ILogger<RedisStockPriceService> _logger;
        private readonly IConfiguration _configuration;

        public RedisStockPriceService(ILogger<RedisStockPriceService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        IDatabase RedisDatabase
        {
            get => ConnectionMultiplexer.Connect(
                _configuration["RedisServerHostname"],
                configure =>
                {
                    configure.Password = _configuration["RedisPassword"];
                }).GetDatabase();
        }

    public async Task<StockPriceUpdate?> ReadStockPrice(string ticker)
    {
        var key = $"stock-price:{ticker}";
        var stockPriceUpdate = await RedisDatabase.StringGetAsync(key);
        if (string.IsNullOrEmpty(stockPriceUpdate))
        {
            return null;
        }

        return JsonConvert.DeserializeObject<StockPriceUpdate?>(stockPriceUpdate);
    }

    public async Task WriteStockPrice(StockPriceUpdate priceUpdate)
        {
            var key = $"stock-price:{priceUpdate.Ticker}";
            var json = JsonConvert.SerializeObject(priceUpdate);
            _logger.LogInformation("Writing stock price update: {Json}", json);
            await RedisDatabase.StringSetAsync(key, json);
        }
    }

    public interface IStockPriceWriteService
    {
        Task WriteStockPrice(StockPriceUpdate priceUpdate);
    }

    public interface IStockPriceReadService
    {
        Task<StockPriceUpdate?> ReadStockPrice(string ticker);
    }
}