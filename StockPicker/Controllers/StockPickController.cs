
using Microsoft.AspNetCore.Mvc;
using StockPicker.Models;
using StockPicker.Services;

namespace StockPicker.Controllers;

public class StockPickController : Controller
{
    private readonly ILogger<StockPickController> _logger;
    private readonly IReadPickListService _readPickListService;

    public StockPickController(ILogger<StockPickController> logger, IReadPickListService readPickListService)
    {
        _logger = logger;
        _readPickListService = readPickListService;
    }

    public async Task<IActionResult> Index()
    {
        var stockPicks = await _readPickListService.GetPickList();
        return View(new StockPickIndexViewModel { StockPicks = stockPicks });
    }

    public IActionResult Create()
    {
        return View();
    }
}
