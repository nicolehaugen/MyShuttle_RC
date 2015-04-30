using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Windows.Networking.PushNotifications;
using MyShuttle.Client.Core.Settings;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;

// http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x409

namespace MyShuttle.Client.UniversalApp
{
    internal class MyShuttleMobileServiceIgnite1_RCPush
    {
        public async static void UploadChannel()
        {
            var channel = await PushNotificationChannelManager
                .CreatePushNotificationChannelForApplicationAsync();

            channel.PushNotificationReceived += channel_PushNotificationReceived;

            try
            {
                await CommonAppSettings.MobileService.GetPush().RegisterNativeAsync(channel.Uri);

            }
            catch (Exception exception)
            {
                HandleRegisterException(exception);
            }
        }

        static void channel_PushNotificationReceived(PushNotificationChannel sender,
            PushNotificationReceivedEventArgs args)
        {
        }

        private static async void HandleRegisterException(Exception exception)
        {
            await ShowAlertAsync(exception.Message, title: "Error");
        }

        private static async Task ShowAlertAsync(string message, string title = "")
        {
            var userInteractionService = Mvx.Resolve<IUserInteraction>();

            if (userInteractionService != null)
            {
                await userInteractionService.AlertAsync(message, title: title);
            }
        }
    }
}
