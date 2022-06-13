using Newtonsoft.Json;

namespace StockDataApi.Models
{
    public class StockTickerViewModel
    {
        public string Ticker { get; set; }

        [JsonProperty("name")]
        public string CompanyName { get; set; }

        [JsonProperty("currency_name")]
        public string Currency { get; set; }

        [JsonProperty("primary_exchange")]
        public string Exchange { get; set; }
        public string Locale { get; set; }
    }
}