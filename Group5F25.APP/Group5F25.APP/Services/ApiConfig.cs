using Microsoft.Maui.Devices; // ADD THIS USING STATEMENT

namespace Group5F25.APP
{
    public static class ApiConfig
    {
        // 🔑 REPLACE the entire static const string with this dynamic property:
        public static string BaseUrl
        {
            get
            {
                // Android Emulator requires 10.0.2.2 and usually works better on HTTP
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    return "http://10.0.2.2:5016/"; // Uses the HTTP port from launchSettings.json
                }

                // Windows, Mac, iOS can use localhost and the HTTPS port
                return "https://localhost:7134/";
            }
        }

        public const string LoginPath = "auth/login";
        public const string RegisterPath = "auth/register";
        public const string MePath = "auth/me";

        public const string TripUploadPath = "api/trip/upload";
        public const string TripUserBasePath = "api/trip/user/";
    }
}