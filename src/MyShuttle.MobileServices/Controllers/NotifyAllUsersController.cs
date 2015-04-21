using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace MyShuttle.MobileServices.Controllers
{
    public class NotifyAllUsersController : ApiController
    {
        public ApiServices Services { get; set; }

        // GET api/NotifyAllUsers
        public string Get()
        {
            Services.Log.Info("Hello from custom controller!");
            return "Hello";
        }


        // The following call is for illustration purpose only. The function
        // body should be moved to a controller in your app where you want
        // to send a notification.
        public async Task<bool> Post(JObject data)
        {
            try
            {
                string wnsToast = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><toast><visual><binding template=\"ToastText01\"><text id=\"1\">{0}</text></binding></visual></toast>", data.GetValue("toast").Value<string>());
                WindowsPushMessage message = new WindowsPushMessage();
                message.XmlPayload = wnsToast;
                var result = await Services.Push.SendAsync(message);
                return true;
            }
            catch (Exception e)
            {
                Services.Log.Error(e.ToString());
            }
            return false;
        }

    }
}
