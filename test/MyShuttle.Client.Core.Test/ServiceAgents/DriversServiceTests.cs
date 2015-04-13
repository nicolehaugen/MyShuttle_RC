using Cirrious.MvvmCross.Plugins.Messenger;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShuttle.Client.Core.DocumentResponse;
using MyShuttle.Client.Core.Infrastructure;
using MyShuttle.Client.Core.ServiceAgents;
using MyShuttle.Client.Core.Tests.ServiceAgents.Fakes;
using System;
using System.Threading.Tasks;

namespace MyShuttle.Client.Core.Tests.ServiceAgents
{
    [TestClass]
    public class DriversServiceTests
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
        public async Task DriversService_Test_Call_All_CRUD_Methods_ShouldWork()
        {
            // Arrange

            // Act
            var actualDriversCount = await _myShuttleClient.DriversService.GetCountAsync(string.Empty);

            var newDriver = new Driver()
            {
                 CarrierId = 1,
                Name = "Name",
                Phone = "Phone",
                Picture = null
            };

            var driverId = await _myShuttleClient.DriversService.PostAsync(newDriver);

            //Assert
            driverId.Should().BeGreaterThan(0);

            var newDriversCount = await _myShuttleClient.DriversService.GetCountAsync(string.Empty);

            newDriversCount.Should().Be(actualDriversCount + 1);

            var currentDriver = await _myShuttleClient.DriversService.GetAsync(driverId);

            //assert
            currentDriver.Should().NotBeNull();

            currentDriver.Name = Guid.NewGuid().ToString();

            await _myShuttleClient.DriversService.PutAsync(currentDriver);

            var updateDriver = await _myShuttleClient.DriversService.GetAsync(driverId);

            updateDriver.Should().NotBeNull();
            updateDriver.Name.Should().Be(currentDriver.Name);

            await _myShuttleClient.DriversService.DeleteAsync(driverId);
            newDriversCount = await _myShuttleClient.DriversService.GetCountAsync(string.Empty);
            newDriversCount.Should().Be(actualDriversCount);

        }
    }

}
