#nullable disable

namespace HuskyEngine.Data.Model
{
    public class Stock
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
