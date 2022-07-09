#nullable disable

namespace HuskyEngine.Data.Model
{
    public class StockHolding
    {
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public string Symbol { get; set; }
        public double? Weight { get; set; }
    }
}
