using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using Microsoft.Office365.Discovery;
using MyShuttle.Client.Core.ServiceAgents.Interfaces;
using MyShuttle.Client.UniversalApp.ConnectedServices;
using MyShuttle.Client.UniversalApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace MyShuttle.Client.UniversalApp.ViewModels
{
    public class RideDetailViewModel : Core.ViewModels.RideDetailViewModel
    {
        IDictionary<string, CapabilityDiscoveryResult> AppCapabilities = null;
        private ICommand _downloadInvoiceCommand;
        public RideDetailViewModel(IMyShuttleClient myShuttleClient, IMvxMessenger messenger)
            : base(myShuttleClient, messenger)
	    {
            InitializeCommands();
	    }

        public ICommand DownloadInvoiceCommand
        {
            get { return this._downloadInvoiceCommand; }
        }

        private void InitializeCommands()
        {
            _downloadInvoiceCommand = new MvxCommand(DownloadInvoice);
        }

        private async  void DownloadInvoice()
        {
            //this.IsLoadingRide = true;
            //var _fileOperations = new InvoiceService();

            //var file = await _fileOperations.GetFile(this.Ride.EmployeeId);
            //if (file != null)
            //{
            //    var launcher = Windows.System.Launcher.LaunchFileAsync(file);
            //}

            //this.IsLoadingRide = false;

            await getAppCapabilities();
            var spClient = SPClient.ensureSPClientCreated(AppCapabilities, "MyFiles");
            var files = await MyFilesOperation.getMyFiles(spClient);
            string listOfFiles = "Files List: ";
            foreach(var myFile in files)
            {
                listOfFiles += Environment.NewLine + myFile.Name;
            }
            MessageDialog dialog = new MessageDialog(listOfFiles);
            await dialog.ShowAsync();
        }

        private async Task getAppCapabilities()
        {
            DiscoveryClient discoveryClient = new DiscoveryClient(
                    async () =>
                    {
                        var authResult = await AuthenticationHelper.GetAccessToken(AuthenticationHelper.DiscoveryServiceResourceId);
                        return authResult.AccessToken;
                    }
                );
            AppCapabilities = await discoveryClient.DiscoverCapabilitiesAsync();
        }
    }
}
