using System;
using System.Net;
using Newtonsoft.Json.Linq;
using StockDataWorker.Models;

namespace StockDataWorker.Services
{
	public class PolygonStockDataQueryService : IStockDataQueryService
	{
        private readonly IConfiguration _configuration;

		public PolygonStockDataQueryService(IConfiguration configuration)
		{
            _configuration = configuration;
		}

        public async Task<IList<StockTicker>> GetTickerData()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiBaseUrl"]);
                var apiKey = _configuration["ApiKey"];

                var urlPart = $"v3/reference/tickers?active=true&sort=ticker&order=asc&limit=50&apiKey={apiKey}";
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

                var responseJsonObject = JObject.Parse(responseContent);
                var resultsEnumerable = responseJsonObject["results"].AsEnumerable();

                return resultsEnumerable
                    .Select(o => o.ToObject<StockTicker>())
                    .ToList();
            }
        }
    }

	public interface IStockDataQueryService
    {
		Task<IList<StockTicker>> GetTickerData();
    }
}

