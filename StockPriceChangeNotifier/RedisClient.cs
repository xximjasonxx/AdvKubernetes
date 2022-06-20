using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using StockPriceChangeNotifier.Models;

namespace StockPriceChangeNotifier
{
    public class RedisClient : IRedisClient
    {
        private const string StockPicksRedisKey = "stock-picks";
        private readonly IConfiguration _configuration;

        public RedisClient(IConfiguration configuration)
        {
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

        public async Task<StockPick?> GetStockPick(string symbol)
        {
            var redisValues = await RedisDatabase.ListRangeAsync(StockPicksRedisKey);
            return redisValues
                .Select(x => JsonConvert.DeserializeObject<StockPick>(x))
                .FirstOrDefault(x => x.Ticker == symbol.ToUpper());
        }
    }

    public interface IRedisClient
    {
        Task<StockPick?> GetStockPick(string symbol);
    }
}