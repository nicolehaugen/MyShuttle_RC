using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShuttle.Client.Core.Infrastructure;
using MyShuttle.Client.Core.ServiceAgents;
using MyShuttle.Client.Core.Tests.ServiceAgents.Fakes;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace MyShuttle.Client.Core.Tests.ServiceAgents
{
    [TestClass]
    public class AnalyticsServiceTests
    {
        // SUT
        static MyShuttleClient _myShuttleClient;

        [ClassInitialize]
        public static void ClassInitialize(TestContext textContext)
        {
            var fakeApplicationDataRepository = new FakeApplicationDataRepository();
            var fakeMessenger = new MvxMessengerHub();
            var applicationSettingService = new ApplicationSettingServiceSingleton(fakeApplicationDataRepository, fakeMessenger);
            var applicationStorageService = new ApplicationStorageService(fakeApplicationDataRepository);

            _myShuttleClient = new MyShuttleClient(applicationSettingService, applicationStorageService, fakeMessenger);
        }

        [TestMethod]
        public async Task AnalyticsService_GetTopDrivers_Should_Retrieve_Top_Drivers()
        {
            // Arrange

            // Act
            var result = await _myShuttleClient.AnalyticsService.GetTopDriversAsync();

            //Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(5);
            result[0].Picture.Should().NotBeNull();
        }

        [TestMethod]
        public async Task AnalyticsService_GetTopVehicles_Should_Retrieve_Top_Vehicles()
        {
            // Arrange

            // Act
            var result = await _myShuttleClient.AnalyticsService.GetTopVehiclesAsync();

            //Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(5);
            result[0].Picture.Should().NotBeNull();
        }

        [TestMethod]
        public async Task AnalyticsService_GetSummary_Should_Retrieve_Summary()
        {
            // Arrange

            // Act
            var result = await _myShuttleClient.AnalyticsService.GetSummaryInfoAsync();

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task AnalyticsService_GetRidesInfo_Should_Retrieve_Rides_Info()
        {
            // Arrange

            // Act
            var result = await _myShuttleClient.AnalyticsService.GetRidesInfoAsync();

            //Assert
            result.Should().NotBeNull();
            result.RidesEvolution.Should().NotBeNull();
        }

    }
}
