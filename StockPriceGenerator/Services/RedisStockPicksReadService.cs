using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace StockPriceGenerator.Services
{
    public class RedisStockPicksReadService : IStockPicksReadService
    {
        private const string StockPicksRedisKey = "stock-picks";
        private readonly ILogger<RedisStockPicksReadService> _logger;
        private readonly IConfiguration _configuration;

        public RedisStockPicksReadService(ILogger<RedisStockPicksReadService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        IDatabase RedisDatabase
        {
            get => ConnectionMultiplexer.Connect(
                _configuration["RedisHostname"],
            configure => {
                configure.Password = _configuration["RedisPassword"];
            }).GetDatabase();
        }

        public async Task<IList<string>> ReadPickedTickers()
        {
            var values = await RedisDatabase.ListRangeAsync(StockPicksRedisKey);
            return values.Select(x => x.ToString()).ToList();
        }
    }

    public interface IStockPicksReadService
    {
        Task<IList<string>> ReadPickedTickers();
    }
}