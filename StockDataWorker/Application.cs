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
        try
        {
            await _sendStockDataSevice.SendStockData();
            Console.WriteLine("Success");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Data Load failed");
            Console.WriteLine(ex.Message);
        }
    }
}
