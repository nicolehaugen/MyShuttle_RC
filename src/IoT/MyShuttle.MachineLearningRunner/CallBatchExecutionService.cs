// This code requires the Nuget package Microsoft.AspNet.WebApi.Client to be installed.
// Instructions for doing this in Visual Studio:
// Tools -> Nuget Package Manager -> Package Manager Console
// Install-Package Microsoft.AspNet.WebApi.Client
//
// Also, add a reference to Microsoft.WindowsAzure.Storage.dll for reading from and writing to the Azure blob storage

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;

namespace MyShuttle.MachineLearningRunner 
{
    public class AzureBlobDataReference
    {
        // Storage connection string used for regular blobs. It has the following format:
        // DefaultEndpointsProtocol=https;AccountName=ACCOUNT_NAME;AccountKey=ACCOUNT_KEY
        // It's not used for shared access signature blobs.
        public string ConnectionString { get; set; }

        // Relative uri for the blob, used for regular blobs as well as shared access 
        // signature blobs.
        public string RelativeLocation { get; set; }

        // Base url, only used for shared access signature blobs.
        public string BaseLocation { get; set; }

        // Shared access signature, only used for shared access signature blobs.
        public string SasBlobToken { get; set; }
    }

    public enum BatchScoreStatusCode
    {
        NotStarted,
        Running,
        Failed,
        Cancelled,
        Finished
    }

    public class BatchScoreStatus
    {
        // Status code for the batch scoring job
        public BatchScoreStatusCode StatusCode { get; set; }


        // Locations for the potential multiple batch scoring outputs
        public IDictionary<string, AzureBlobDataReference> Results { get; set; }

        // Error details, if any
        public string Details { get; set; }
    }

    public class BatchScoreRequest
    {
        public AzureBlobDataReference Input { get; set; }
        public IDictionary<string, string> GlobalParameters { get; set; }
    }

    public static class CallBatchExecutionService
    {
        public static async Task InvokeBatchExecutionService()
        {
            string BaseUrl = ConfigurationManager.AppSettings["MLBaseUrl"];
            string StorageContainerName = ConfigurationManager.AppSettings["StorageContainerName"];
            string InputBlobName = ConfigurationManager.AppSettings["InputBlobName"];
            string apiKey = ConfigurationManager.AppSettings["MLApiKey"];
            string storageConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;

            var blobClient = CloudStorageAccount.Parse(storageConnectionString).CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(StorageContainerName);
            var blob = container.GetBlockBlobReference(InputBlobName);

            Trace.WriteLine("Submitting the job...");

            // set a time out for polling status
            const int TimeOutInMilliseconds = 120 * 1000; // Set a timeout of 2 minutes

            using (HttpClient client = new HttpClient())
            {
                BatchScoreRequest request = new BatchScoreRequest()
                {
                    Input = new AzureBlobDataReference()
                    {
                        ConnectionString = storageConnectionString,
                        RelativeLocation = blob.Uri.LocalPath
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var response = await client.PostAsJsonAsync(BaseUrl, request);
                string jobId = await response.Content.ReadAsAsync<string>();

                string jobLocation = BaseUrl + "/" + jobId + "?api-version=2.0";
                Stopwatch watch = Stopwatch.StartNew();
                bool done = false;
                while (!done)
                {
                    response = await client.GetAsync(jobLocation);
                    BatchScoreStatus status = await response.Content.ReadAsAsync<BatchScoreStatus>();
                    if (watch.ElapsedMilliseconds > TimeOutInMilliseconds)
                    {
                        done = true;
                        Console.WriteLine("Timed out. Deleting the job ...");
                        await client.DeleteAsync(jobLocation);
                    }
                    switch (status.StatusCode)
                    {
                        case BatchScoreStatusCode.NotStarted:
                            Trace.WriteLine("Not started...");
                            break;
                        case BatchScoreStatusCode.Running:
                            Trace.WriteLine("Running...");
                            break;
                        case BatchScoreStatusCode.Failed:
                            Trace.WriteLine("Failed!");
                            Trace.WriteLine(string.Format("Error details: {0}", status.Details));
                            done = true;
                            break;
                        case BatchScoreStatusCode.Cancelled:
                            Trace.WriteLine("Cancelled!");
                            done = true;
                            break;
                        case BatchScoreStatusCode.Finished:
                            done = true;
                            Trace.WriteLine("Finished!");
                            break;
                    }

                    if (!done)
                    {
                        Thread.Sleep(1000); // Wait one second
                    }
                }
            }
        }
    }
}
