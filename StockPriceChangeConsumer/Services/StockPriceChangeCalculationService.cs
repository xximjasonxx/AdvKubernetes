namespace StockPriceChangeConsumer.Services
{
    public class StockPriceChangeCalculationService
    {
        public decimal GetPercentDifference(decimal oldPrice, decimal newPrice)
        {
            var result = (newPrice - oldPrice) / oldPrice;
            return Math.Round(result, 2);
        }
    }
}