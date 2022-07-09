#nullable disable

namespace HuskyEngine.Data.Model
{
    public class FinancialFactorDatum
    {
        public long Id { get; set; }
        public string Symbol { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime EndDate { get; set; }
        public double? Value { get; set; }
    }
}
