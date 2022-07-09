#nullable disable

namespace HuskyEngine.Data.Model
{
    public class Factor
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Formula { get; set; }
        public int SourceType { get; set; }
        public int? Type { get; set; }
        public int ValueType { get; set; }
        public DateTime? DataStart { get; set; }
        public DateTime? DataEnd { get; set; }
    }
}
