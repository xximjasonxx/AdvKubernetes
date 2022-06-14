
using Microsoft.AspNetCore.Mvc;
using StockPicker.Common.Ex;
using StockPicker.Models;
using StockPicker.Services;

namespace StockPicker.Controllers;

public class StockPickController : Controller
{
    private readonly ILogger<StockPickController> _logger;
    private readonly IReadPickListService _readPickListService;
    private readonly IWritePickService _writePickService;
    private readonly StockDataApiService _stockDataApiService;

    public StockPickController(ILogger<StockPickController> logger, IReadPickListService readPickListService,
        IWritePickService writePickService, StockDataApiService stockDataApiService)
    {
        _logger = logger;
        _readPickListService = readPickListService;
        _writePickService = writePickService;
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

    [HttpGet("{symbol}")]
    public async Task<IActionResult> Delete(string symbol)
    {
        await _writePickService.DeletePick(symbol);
        return RedirectToAction("Index");
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

        try
        {
            await _writePickService.AddPick(viewModel.SymbolToTrack);
            return RedirectToAction("Index");
        }
        catch (DuplicateSymbolPickException ex)
        {
            ModelState.AddModelError("SymbolToTrack", ex.Message);
            return View(viewModel);
        }
    }
}
