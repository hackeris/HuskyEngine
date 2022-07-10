#nullable disable

namespace HuskyEngine.Data.Model
{
    public class FactorDatum : IFactorDatum
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Symbol { get; set; }
        public double? Value { get; set; }
    }
}