using System.ComponentModel.DataAnnotations;

namespace StockPicker.Models
{
    public class StockPickCreateViewModel
    {
        [Required]
        [Display(Name = "Stock Symbol to Track")]
        public string SymbolToTrack { get; set; }
    }
}