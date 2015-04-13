using Cirrious.MvvmCross.Plugins.Messenger;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShuttle.Client.Core.Infrastructure;
using MyShuttle.Client.Core.ServiceAgents;
using MyShuttle.Client.Core.Tests.ServiceAgents.Fakes;
using System.Threading.Tasks;

namespace MyShuttle.Client.Core.Tests.ServiceAgents
{
    [TestClass]
    public class EmployeesServiceTests
    {
        // SUT
        static MyShuttleClient _myShuttleClient;

        [ClassInitialize]
        public static void ClassInitialize(TestContext textContext)
        {
            // The url is set in the FakeApplicationDataRepository
            var fakeApplicationDataRepository = new FakeApplicationDataRepository();
            var fakeMessenger = new MvxMessengerHub();
            var applicationSettingService = new ApplicationSettingServiceSingleton(fakeApplicationDataRepository, fakeMessenger);
            var applicationStorageService = new ApplicationStorageService(fakeApplicationDataRepository);
            
            _myShuttleClient = new MyShuttleClient(applicationSettingService, applicationStorageService, fakeMessenger);
        }

        [TestMethod]
        public async Task EmployeesService_Test_Call_GetMyProfile_Should_Get_Employee()
        {
            var results = await _myShuttleClient.EmployeesService.GetMyProfileAsync();

            results.Should().NotBeNull();
        }

    }

}
