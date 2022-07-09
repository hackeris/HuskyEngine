#nullable disable

namespace HuskyEngine.Data.Model
{
    public class User
    {
        public long Id { get; set; }
        public string Password { get; set; }
        public string SocketToken { get; set; }
        public string Username { get; set; }
    }
}