namespace StockDataWorker.Services
{
    public class SendStockDataService
    {
        private readonly IStockDataQueryService _stockDataQueryService;
        private readonly StockDataGenerationService _stockDataGenerationService;
        private readonly IDataSendService _dataSendService;

        public SendStockDataService(IStockDataQueryService stockDataQueryService,
            StockDataGenerationService stockDataGenerationService,
            IDataSendService dataSendService)
        {
            _stockDataQueryService = stockDataQueryService;
            _stockDataGenerationService = stockDataGenerationService;
            _dataSendService = dataSendService;
        }

        public async Task SendStockData()
        {
            var tickers = await _stockDataQueryService.GetTickerData();

            // generate the stock data we are going to send
            var stockPriceData = _stockDataGenerationService.GetStockPrices(tickers).ToList();

            // send the data to the event stream
            await _dataSendService.SendStockPriceData(stockPriceData);
        }
    }
}