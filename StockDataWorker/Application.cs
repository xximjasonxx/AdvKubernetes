using StockDataWorker.Services;

namespace StockDataWorker;

public class Application
{
    private readonly ILogger<Application> _logger;
    private readonly SendStockDataService _sendStockDataSevice;

    public Application(ILogger<Application> logger,
        SendStockDataService sendStockDataService)
    {
        _logger = logger;
        _sendStockDataSevice = sendStockDataService;
    }

    public async Task Execute()
    {
        var task = _sendStockDataSevice.SendStockData();
        await Task.WhenAll(new [] { task });

        if (task.IsFaulted)
        {
            Console.WriteLine("Data Load failed");
        }
        else
        {
            Console.WriteLine("Data Load Succeeded");
        }
    }
}
