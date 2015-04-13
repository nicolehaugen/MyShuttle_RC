using MyShuttle.Client.Core.Infrastructure;
using MyShuttle.Client.Core.ServiceAgents;
using MyShuttle.Client.Core.Tests.ServiceAgents.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShuttle.Client.Core.DocumentResponse;
using System;
using System.Threading.Tasks;
using System.Linq;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace MyShuttle.Client.Core.Tests.ServiceAgents
{
    [TestClass]
    public class RidesServiceTests
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
        public async Task RidesService_Test_Call_GetAsync_Should_Return_Value()
        {
            // Arrange

            // Act
            var result = await _myShuttleClient.RidesService.GetAsync(1);

            //Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task RidesService_Test_Call_GetCount_Should_Return_Value()
        {
            // Arrange

            // Act
            var result = await _myShuttleClient.RidesService
                .GetCountAsync(null, null);

            //Assert
            result.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public async Task RidesService_Test_Call_Get_Should_Return_One_Ride()
        {
            // Arrange

            // Act
            var result = await _myShuttleClient.RidesService
                .GetAsync(null, null, 1, 0);

            //Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(1);
        }

        [TestMethod]
        public async Task RidesService_Test_Call_GetMyRides_Should_Return_One_Value()
        {
            // Arrange

            // Act
            var result = await _myShuttleClient.RidesService.GetMyRidesAsync(1);

            //Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(1);
        }

        [TestMethod]
        public async Task RidesService_Test_Call_GetMyCompanyRides_Should_Return_One_Value()
        {
            // Arrange

            // Act
            var result = await _myShuttleClient.RidesService.GetCompanyRidesAsync(1);

            //Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(1);
        }

        [TestMethod]
        public async Task RidesService_Test_Call_Update_Should_Be_Updated()
        {
            // Arrange

            // Act
            Ride ride = await _myShuttleClient.RidesService.GetAsync(1);

            ride.Comments = Guid.NewGuid().ToString();

            await _myShuttleClient.RidesService.PutAsync(ride);

            var updateRide = await _myShuttleClient.RidesService.GetAsync(1);

            //Assert
            updateRide.Comments.Should().Be(ride.Comments);
        }
    }

}
