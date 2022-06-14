namespace StockPicker.Common.Ex
{
    public class DuplicateSymbolPickException : Exception
    {
        public string DuplicateSymbol { get; private set; }

        public DuplicateSymbolPickException(string symbol) : base($"Duplicate symbol: {symbol}")
        {
            DuplicateSymbol = symbol;
        }
    }
}