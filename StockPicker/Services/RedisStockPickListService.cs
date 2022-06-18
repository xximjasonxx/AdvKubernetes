using Newtonsoft.Json;
using StackExchange.Redis;
using StockPicker.Common.Ex;
using StockPicker.Models;

namespace StockPicker.Services
{
    public class RedisStockPickListService : IReadPickListService, IWritePickService
    {
        private const string StockPicksRedisKey = "stock-picks";

        private readonly IConfiguration _configuration;

        IDatabase RedisDatabase
        {
            get => ConnectionMultiplexer.Connect(
                _configuration["RedisServerHostname"],
                configure =>
                {
                    configure.Password = _configuration["RedisPassword"];
                }).GetDatabase();
        }

        public RedisStockPickListService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<StockPick>> GetPickList()
        {
            var redisValues = await RedisDatabase.ListRangeAsync(StockPicksRedisKey);
            return redisValues.Select(x => JsonConvert.DeserializeObject<StockPick>(x.ToString())).ToList();
        }

        public async Task AddPick(StockPick stockPick)
        {
            var redisValues = await GetPickList();
            if (redisValues.Any(x => x.Ticker == stockPick.Ticker))
            {
                throw new DuplicateSymbolPickException(stockPick.Ticker);
            }

            await RedisDatabase.ListRightPushAsync(StockPicksRedisKey, JsonConvert.SerializeObject(stockPick));
        }

        public async Task DeletePick(string symbol)
        {
            var redisValues = await GetPickList();
            var stockPick = redisValues.FirstOrDefault(x => x.Ticker == symbol);
            if (stockPick != null)
            {
                await RedisDatabase.ListRemoveAsync(StockPicksRedisKey, JsonConvert.SerializeObject(stockPick));
            }
        }
  }

    public interface IReadPickListService
    {
        Task<List<StockPick>> GetPickList();
    }

    public interface IWritePickService
    {
        Task AddPick(StockPick stockPick);
        Task DeletePick(string symbol);
    }
}