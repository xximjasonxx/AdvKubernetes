using Newtonsoft.Json.Linq;
using StockDataApi.Models;

namespace StockDataApi.Services
{
    public class PolygonStockDataService : IStockDataService
    {
        private readonly IConfiguration _configuration;

        public PolygonStockDataService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<StockTickerViewModel?> GetStockTicker(string symbol)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiBaseUrl"]);
                var apiKey = _configuration["ApiKey"];

                var urlPart = $"v3/reference/tickers?active=true&ticker={symbol.ToUpper()}&apiKey={apiKey}";
                Console.WriteLine($"urlPart: {urlPart}");
                var response = await client.GetAsync(urlPart);
                if (response.IsSuccessStatusCode == false)
                {
                    throw new InvalidOperationException("Get Data Request Failed");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseContent))
                {
                    throw new InvalidOperationException("Get Data Request returned no data");
                }

                return JObject.Parse(responseContent)["results"]?
                    .AsEnumerable()
                    .Select(o => o.ToObject<StockTickerViewModel>())
                    .FirstOrDefault();
            }
        }
    }

    public interface IStockDataService
    {
        Task<StockTickerViewModel?> GetStockTicker(string symbol);
    }
}