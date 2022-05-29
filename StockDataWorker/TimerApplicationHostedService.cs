using StockDataWorker.Services;

namespace StockDataWorker;

public class TimerApplicationHostedService : IHostedService
{
    private readonly ILogger<TimerApplicationHostedService> _logger;
    private readonly SendStockDataService _sendStockDataSevice;
    private Timer _timer = null!;

    public TimerApplicationHostedService(ILogger<TimerApplicationHostedService> logger,
        SendStockDataService sendStockDataService)
    {
        _logger = logger;
        _sendStockDataSevice = sendStockDataService;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        _timer = new Timer(SendStockData, null, TimeSpan.Zero, 
            TimeSpan.FromMinutes(5));

        return Task.CompletedTask;
    }

    async void SendStockData(object? state)
    {
        await _sendStockDataSevice.SendStockData();
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
