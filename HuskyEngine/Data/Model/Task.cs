#nullable disable

namespace HuskyEngine.Data.Model
{
    public class Task
    {
        public string Id { get; set; }
        public string Parameters { get; set; }
        public float? Progress { get; set; }
        public string TaskResult { get; set; }
        public int? TaskStatus { get; set; }
        public string TaskType { get; set; }
        public long? UserId { get; set; }
    }
}
