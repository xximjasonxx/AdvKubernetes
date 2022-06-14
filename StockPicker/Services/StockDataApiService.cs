namespace StockPicker.Services
{
    public class StockDataApiService
    {
        public readonly IConfiguration _configuration;

        public StockDataApiService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> IsStockSymbolValid(string stockSymbol)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(_configuration["StockDataApiUrl"]);

            var response = await client.GetAsync($"/stockticker/{stockSymbol}");
            return response.IsSuccessStatusCode;
        }
    }
}