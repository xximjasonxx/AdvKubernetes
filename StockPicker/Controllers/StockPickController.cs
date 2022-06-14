
using Microsoft.AspNetCore.Mvc;
using StockPicker.Models;
using StockPicker.Services;

namespace StockPicker.Controllers;

public class StockPickController : Controller
{
    private readonly ILogger<StockPickController> _logger;
    private readonly IReadPickListService _readPickListService;
    private readonly StockDataApiService _stockDataApiService;

    public StockPickController(ILogger<StockPickController> logger, IReadPickListService readPickListService,
        StockDataApiService stockDataApiService)
    {
        _logger = logger;
        _readPickListService = readPickListService;
        _stockDataApiService = stockDataApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var stockPicks = await _readPickListService.GetPickList();
        return View(new StockPickIndexViewModel { StockPicks = stockPicks });
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(StockPickCreateViewModel viewModel)
    {
        if (ModelState.IsValid == false)
        {
            return View(viewModel);
        }

        // is this a valid symbol
        var isValid = await _stockDataApiService.IsStockSymbolValid(viewModel.SymbolToTrack);
        if (isValid == false)
        {
            ModelState.AddModelError("SymbolToTrack", "Invalid stock symbol");
            return View(viewModel);
        }

        return RedirectToAction("Index");
    }
}
