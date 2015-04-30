using Microsoft.WindowsAzure.MobileServices;
namespace MyShuttle.Client.Core.Settings
{
    public static class CommonAppSettings
    {
        //Test Service
        //private readonly static string _MobileServiceUrl = "http://localhost:49774/";

        private readonly static string _MobileServiceUrl = "https://myshuttlepushnotificationmobileservice.azure-mobile.net/";
        private readonly static string _MobileServiceKey = "muLzKtUgItkcGCSRkvxovHKaygpRSS90";

        //private static readonly string _SignalRUrl = "http://myshuttle.azurewebsites.net/web/";

        private static readonly string _SignalRUrl = "http://myshuttlebiz1ignite-rc.azurewebsites.net/";

        private static MobileServiceClient mobileService = null;

        //Jay's employee id
        private readonly static string _FixedEmployeeId = "90a0d28b-4342-40d5-92b6-af2d0ea7630b";

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
                    if (CommonAppSettings.MobileServiceUrl.Contains("localhost"))
                    {
                        //NLH - When debugging locally, only specify the local url
                        mobileService = new MobileServiceClient(CommonAppSettings.MobileServiceUrl);
                    }
                    else
                    {
                        mobileService = new MobileServiceClient(CommonAppSettings.MobileServiceUrl,
                            CommonAppSettings.MobileServiceKey);
                    }
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
