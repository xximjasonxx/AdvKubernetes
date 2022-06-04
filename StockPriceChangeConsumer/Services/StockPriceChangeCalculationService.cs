namespace StockPriceChangeConsumer.Services
{
    public class StockPriceChangeCalculationService
    {
        public decimal GetPercentDifference(decimal oldPrice, decimal newPrice)
        {
            return (newPrice - oldPrice) / oldPrice;
        }
    }
}