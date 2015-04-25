using Microsoft.Office365.SharePoint.CoreServices;
using Microsoft.Office365.SharePoint.FileServices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyShuttle.Client.UniversalApp
{
    static class ConnectedService
    {
        const string ServiceResourceId = "https://mycompanydemo.sharepoint.com/";
        static readonly Uri ServiceEndpointUri = new Uri("https://mycompanydemo.sharepoint.com/_api/");

        static string _lastLoggedInUser;

        static internal String LoggedInUser
        {
            get
            {
                return _lastLoggedInUser;
            }
        }

        public static async Task GetInvoiceAsync(int employeeId)
        {
            //var client = await EnsureClientCreated();
            //if (client == null)
            //    return null;

            //// Obtain files in default SharePoint folder
            //var filesResults = await client.Files.ExecuteAsync();

            //var file = filesResults.CurrentPage.FirstOrDefault(f => f
            //   .Name.Equals(String.Format("invoice_{0}.pdf", employeeId)));

            //return (File)file;
        }

        public static async Task EnsureClientCreated()
        {
            //if (_discoveryContext == null)
            //{
            //    _discoveryContext = await DiscoveryContext.CreateAsync();
            //}

            //ResourceDiscoveryResult dcr = null;
            //try
            //{
            //    dcr = await _discoveryContext.DiscoverResourceAsync(ServiceResourceId);
            //}
            //catch (AuthenticationFailedException)
            //{
            //    // If the user go back in the login page an Authentication exception is launched.
            //    return null;
            //}

            //_lastLoggedInUser = dcr.UserId;

            //return new SharePointClient(ServiceEndpointUri, async () =>
            //{
            //    return (await _discoveryContext.AuthenticationContext
            //        .AcquireTokenSilentAsync(ServiceResourceId, _discoveryContext.AppIdentity.ClientId, new Microsoft.IdentityModel.Clients.ActiveDirectory.UserIdentifier(dcr.UserId, Microsoft.IdentityModel.Clients.ActiveDirectory.UserIdentifierType.UniqueId))).AccessToken;
            //});
        }

        public static async Task SignOut()
        {
            if (string.IsNullOrEmpty(_lastLoggedInUser))
            {
                return;
            }

            //if (_discoveryContext == null)
            //{
            //    _discoveryContext = await DiscoveryContext.CreateAsync();
            //}

            //await _discoveryContext.LogoutAsync(_lastLoggedInUser);
        }

    }
}
