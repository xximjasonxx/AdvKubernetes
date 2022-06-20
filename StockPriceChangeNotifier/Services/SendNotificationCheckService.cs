using Microsoft.Extensions.Logging;

namespace StockPriceChangeNotifier.Services
{
    public class SendNotificationCheckService
    {
        private readonly IRedisClient _redisClient;
        private readonly ILogger<SendNotificationCheckService> _logger;

        public SendNotificationCheckService(IRedisClient redisClient, ILogger<SendNotificationCheckService> logger)
        {
            _redisClient = redisClient;
            _logger = logger;
        }
        
        public async Task<bool> ShouldSendNotification(string ticker, decimal percentChange)
        {
            var stockPick = await _redisClient.GetStockPick(ticker);
            if (stockPick == null)
            {
                _logger.LogInformation("No stock pick found for {Ticker}", ticker);
                throw new Exception($"No stock pick found for {ticker}");
            }

            if (stockPick.ChangeThreshold.HasValue == false || Math.Abs(percentChange) > stockPick.ChangeThreshold.Value)
                return false;

            return true;
        }
    }
}