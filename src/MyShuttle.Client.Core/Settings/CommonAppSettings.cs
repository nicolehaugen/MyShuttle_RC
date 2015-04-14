using Microsoft.WindowsAzure.MobileServices;
namespace MyShuttle.Client.Core.Settings
{
    public static class CommonAppSettings
    {
        //private readonly static string _MobileServiceUrl = "http://localhost:35256/";
        private readonly static string _MobileServiceUrl = "https://myshuttlemobileserviceignite1-rc.azure-mobile.net/";
        private readonly static string _MobileServiceKey = "RgNJIxnKylcQcvXiCIsFWqACToAntZ27";

        //private static readonly string _SignalRUrl = "http://myshuttle.azurewebsites.net/web/";

        private static readonly string _SignalRUrl = "http://myshuttlebiz1ignite-rc.azurewebsites.net/";

        private static MobileServiceClient mobileService = null;

        private readonly static string _FixedEmployeeId = "c7a9c8a2-a5e3-48d4-99e4-4117599af048";

        public static string MobileServiceUrl
        {
            get
            {
                return _MobileServiceUrl;
            }
        }

        public static string SignalRUrl
        {
            get { return _SignalRUrl; }
        }

        public static string MobileServiceKey
        {
            get
            {
                return _MobileServiceKey;
            }
        }

        public static MobileServiceClient MobileService
        {
            get
            {
                if (mobileService == null)
                {
                    mobileService = new MobileServiceClient(CommonAppSettings.MobileServiceUrl,
                        CommonAppSettings.MobileServiceKey);
                }

                return mobileService;
            }
        }

        public static string FixedEmployeeId
        {
            get
            {
                return _FixedEmployeeId;
            }
        }
    }
}
