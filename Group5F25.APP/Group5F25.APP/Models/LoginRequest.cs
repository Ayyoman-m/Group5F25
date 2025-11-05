
namespace Group5F25.APP.Models
{
    // DummyJSON expects username + password
    public sealed class LoginRequest
    {
        public string email { get; set; } = "";
        public string password { get; set; } = "";
    }
}
