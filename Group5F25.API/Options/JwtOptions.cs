namespace Group5F25.API.Options
{
    public class JwtOptions
    {
        public string Key { get; set; } = "TEST_SECRET_KEY_SHOULD_BE_32+_CHARS_LONG_123456";
        public string Issuer { get; set; } = "Group5F25.API";
        public string Audience { get; set; } = "Group5F25.Client";
        public int ExpiresMinutes { get; set; } = 60;
    }
    // second commit 

}
