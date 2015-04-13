using MyShuttle.Client.Core.Infrastructure.Abstractions.Repositories;

namespace MyShuttle.Client.Core.Tests.ServiceAgents.Fakes
{
    public class FakeApplicationDataRepository : IApplicationDataRepository
    {
        public string GetStringFromApplicationData(string key, string defaultValue)
        {
            return "http://localhost:5000/";
        }

        public bool? GetOptionalBooleanFromApplicationData(string key, bool? defaultValue)
        {
            return true;
        }
        
        public int? GetOptionalIntegerFromApplicationData(string key, int? defaultValue)
        {
            return 5;
        }

        public void SetStringToApplicationData(string UrlPrefixKey, string value)
        {
            // Do nothing
        }
    }
}
