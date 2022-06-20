namespace StockPriceChangeNotifier.Models
{
    public class StockPick
    {
        public string Ticker { get; set; }
        public decimal? ChangeThreshold { get; set; }
        public bool NotifyOnAll { get; internal set; }
    }
}