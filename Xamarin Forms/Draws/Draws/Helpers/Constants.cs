using Xamarin.Essentials;

namespace Draws.Helpers
{
    public static class Constants
    {
        public const string AzureBackendUrl = "https://drawsskiasharpddpu.service.signalr.net";

        public const string iOSLocalBackendUrl = "http://localhost:7071/api";

        public const string AndroidLocalBackendUrl = "http://10.0.2.2:7071/api";
       
        
        public static string BackendUrl
        {
            get
            {
                return DeviceInfo.Platform == DevicePlatform.iOS 
                    ? iOSLocalBackendUrl 
                    : "http://192.168.1.66:7071/api";
            }
        }

        public static string UserName = nameof(UserName);
    }
}