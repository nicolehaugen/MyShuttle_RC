using Microsoft.Office365.Discovery;
using Microsoft.Office365.SharePoint.CoreServices;
using System.Collections.Generic;
using System.Linq;

namespace MyShuttle.Client.UniversalApp.Services
{
    static class SPClient
    {
        public static SharePointClient ensureSPClientCreated(IDictionary<string, CapabilityDiscoveryResult> appCapabilities, string capability)
        {
            var myFilesCapability = appCapabilities
                                        .Where(s => s.Key == capability)
                                        .Select(p => new { Key = p.Key, ServiceResourceId = p.Value.ServiceResourceId, ServiceEndPointUri = p.Value.ServiceEndpointUri })
                                        .FirstOrDefault();

            SharePointClient spClient = new SharePointClient(myFilesCapability.ServiceEndPointUri,
                     async () =>
                     {
                         var authResult = await AuthenticationHelper.GetAccessToken(myFilesCapability.ServiceResourceId);
                         return authResult.AccessToken;
                     });

            return spClient;
        }

    }
}
