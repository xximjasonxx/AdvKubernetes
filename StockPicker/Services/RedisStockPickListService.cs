using StackExchange.Redis;
using StockPicker.Common.Ex;

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

        public async Task<List<string>> GetPickList()
        {
            var redisValues = await RedisDatabase.ListRangeAsync(StockPicksRedisKey);
            return redisValues.Select(x => x.ToString()).ToList();
        }

        public async Task AddPick(string symbol)
        {
            var redisValues = await GetPickList();
            if (redisValues.Contains(symbol))
            {
                throw new DuplicateSymbolPickException(symbol);
            }

            await RedisDatabase.ListRightPushAsync(StockPicksRedisKey, symbol.ToUpper());
        }

        public async Task DeletePick(string symbol)
        {
            var redisValues = await GetPickList();
            if (redisValues.Contains(symbol))
            {
                await RedisDatabase.ListRemoveAsync(StockPicksRedisKey, symbol);
            }
        }
  }

    public interface IReadPickListService
    {
        Task<List<string>> GetPickList();
    }

    public interface IWritePickService
    {
        Task AddPick(string symbol);
        Task DeletePick(string symbol);
    }
}