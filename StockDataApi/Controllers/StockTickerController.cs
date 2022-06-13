using Microsoft.AspNetCore.Mvc;
using StockDataApi.Services;

namespace StockDataApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StockTickerController : ControllerBase
{
    private readonly ILogger<StockTickerController> _logger;
    private readonly IStockDataService _stockDataSevice;

    public StockTickerController(ILogger<StockTickerController> logger,
        IStockDataService stockDataService)
    {
        _logger = logger;
        _stockDataSevice = stockDataService;
    }

    [HttpGet("{symbol}")]
    public async Task<IActionResult> Get(string symbol)
    {
        var stockTicker = await _stockDataSevice.GetStockTicker(symbol);
        if (stockTicker == null)
        {
            return NotFound();
        }

        return Ok(stockTicker);
    }
}
