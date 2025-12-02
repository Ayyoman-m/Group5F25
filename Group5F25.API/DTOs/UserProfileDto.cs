namespace Group5F25.API.DTOs
{
    // Profile data returned by GET /auth/me
    public class UserProfileDto
    {
        public int id { get; set; }
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
    }
}
