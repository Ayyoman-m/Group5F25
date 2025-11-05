namespace Group5F25.APP.Models
{
    public sealed class UserProfile
    {
        public int id { get; set; }
        public string username { get; set; } = "";
        public string email { get; set; } = "";
        public string firstName { get; set; } = "";
        public string lastName { get; set; } = "";
        public string role { get; set; } = "";
    }
}

