namespace StockPriceChangeConsumer.Services
{
    public class CosmosSqlStockPriceLookupService : IStockPriceLookupService
    {
        public Task<StockPrice> GetStockPrice(string ticker)
        {
            throw new NotImplementedException();
        }
    }

    public interface IStockPriceLookupService
    {
        Task<StockPrice> GetStockPrice(string ticker);
    }
}