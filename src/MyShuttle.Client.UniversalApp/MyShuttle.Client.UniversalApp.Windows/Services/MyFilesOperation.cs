using Microsoft.Office365.SharePoint.CoreServices;
using Microsoft.Office365.SharePoint.FileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShuttle.Client.UniversalApp.Services
{
    static class MyFilesOperation
    {
        public static async Task<List<IItem>> getMyFiles(SharePointClient spClient)
        {
            List<IItem> myFilesList = new List<IItem>();
            var myFilesResult = await spClient.Files.ExecuteAsync();
            do
            {
                var files = myFilesResult.CurrentPage;
                foreach (var file in files)
                {
                    myFilesList.Add(file);
                }
                myFilesResult = await myFilesResult.GetNextPageAsync();
            } while (myFilesResult != null);
            return myFilesList;
        }

    }
}
