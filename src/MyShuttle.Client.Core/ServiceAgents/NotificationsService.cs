using MyShuttle.Client.Core.Settings;
using MyShuttle.Client.Core.Web;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShuttle.Client.Core.ServiceAgents
{
    internal class NotificationsService : BaseRequest, INotificationsService
    {
        public NotificationsService(string urlPrefix, string securityToken)
            : base(urlPrefix, securityToken)
        {
        }

        public async Task RequestVehicleAsync(string employeeId, int driverId, double latitude, double longitude)
        {
            try
            {
                await CommonAppSettings.MobileService.GetTable("NotificationTable").InsertAsync(new JObject(new JProperty("text", employeeId), new JProperty("complete", "true")));
            }
            catch (Exception e)
            {
                string s = e.Message;
            }
        }
    }
}
