using StackExchange.Redis;

namespace StockPicker.Services
{
    public class RedisStockPickListService : IReadPickListService
    {
        private const string StockPicksRedisKey = "stock-picks";

        private readonly IConfiguration _configuration;

        public RedisStockPickListService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<string>> GetPickList()
        {
            var redisConnection = ConnectionMultiplexer.Connect(_configuration["RedisServerHostname"]);
            var database = redisConnection.GetDatabase();
            var redisValues = await database.ListRangeAsync(StockPicksRedisKey);

            return redisValues.Select(x => x.ToString()).ToList();
        }
    }

    public interface IReadPickListService
    {
        Task<List<string>> GetPickList();
    }
}