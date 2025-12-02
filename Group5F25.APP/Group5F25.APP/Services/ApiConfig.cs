namespace Group5F25.APP
{
    public static class ApiConfig
    {
        public const string BaseUrl = "https://localhost:7134/";
        public const string LoginPath = "auth/login";
        public const string RegisterPath = "auth/register"; 
        public const string MePath = "auth/me"; // only if backend adds it

        public const string TripUploadPath = "api/trip/upload";
        public const string TripUserBasePath = "api/trip/user/"; // for history later
    }
}
