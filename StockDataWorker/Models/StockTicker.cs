using System;
using Newtonsoft.Json;

namespace StockDataWorker.Models
{
	public class StockTicker
	{
		public string Ticker { get; set; }

        [JsonProperty("name")]
		public string CompanyName { get; set; }

        [JsonProperty("primary_exchange")]
		public string Exchange { get; set; }

        [JsonProperty("currency_name")]
		public string Currency { get; set; }

		public string Market { get; set; }

		public StockTicker()
		{
		}
	}
}

