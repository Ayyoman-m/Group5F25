namespace Group5F25.APP
{
    public static class ApiConfig
    {
        public const string BaseUrl = "https://localhost:7134/";
        public const string LoginPath = "auth/login";
        public const string RegisterPath = "auth/register"; // optional, for later
        public const string MePath = "auth/me"; // only if backend adds it
    }
}
