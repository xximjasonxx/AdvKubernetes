using Newtonsoft.Json;

namespace StockPriceChangeConsumer.Entities
{
    public class StockPrice : EntityBase
    {
        [JsonProperty(PropertyName = "id")]
        public override string Id => Ticker;

        [JsonProperty(PropertyName = "ticker")]
        public string Ticker { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}