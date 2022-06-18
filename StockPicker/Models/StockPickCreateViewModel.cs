using System.ComponentModel.DataAnnotations;

namespace StockPicker.Models
{
    public class StockPickCreateViewModel
    {
        [Required]
        [Display(Name = "Stock Symbol")]
        public string SymbolToTrack { get; set; }

        [Display(Name = "Notify on all price changes")]
        public bool NotifyOnAllPriceChanges { get; set; }

        [Display(Name = "Notify on price change percentage")]
        public decimal? PriceChangePercentThreshold { get; set; }
    }
}