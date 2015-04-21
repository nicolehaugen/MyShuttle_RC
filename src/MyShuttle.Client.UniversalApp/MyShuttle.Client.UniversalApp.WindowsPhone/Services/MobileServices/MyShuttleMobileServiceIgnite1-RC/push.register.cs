using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

// http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x409

namespace MyShuttle.Client.UniversalApp
{
    internal class MyShuttleMobileServiceIgnite1_RCPush
    {
        //NLH - This push notification was added for debugging purposes only
        public async static void UploadChannel()
        {
            var channel = await Windows.Networking.PushNotifications.PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            try
            {
                await App.MyShuttleMobileServiceIgnite1_RCClient.GetPush().RegisterNativeAsync(channel.Uri);
                await App.MyShuttleMobileServiceIgnite1_RCClient.InvokeApiAsync("notifyAllUsers",
                       new JObject(new JProperty("toast", "Sample Toast")));
            }
            catch (Exception exception)
            {
                HandleRegisterException(exception);
            }
        }

        private static void HandleRegisterException(Exception exception)
        {

        }
    }
}
