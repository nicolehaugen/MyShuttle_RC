using Cirrious.MvvmCross.Plugins.Messenger;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShuttle.Client.Core.DocumentResponse;
using MyShuttle.Client.Core.Infrastructure;
using MyShuttle.Client.Core.ServiceAgents;
using MyShuttle.Client.Core.Tests.ServiceAgents.Fakes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyShuttle.Client.Core.Tests.ServiceAgents
{
    [TestClass]
    public class VehiclesServiceTests
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
        public async Task VehiclesService_Test_Call_All_CRUD_Methods_ShouldWork()
        {
            // Arrange

            // Act
            var actualVehicleCount = await _myShuttleClient.VehiclesService.GetCountAsync(string.Empty);

            var newVehicle = new Vehicle()
            {
                LicensePlate = "SLV-4335",
                Model = "Silver Bullet",
                Make = "Silver Bullet Factory",
                Type = VehicleType.Compact,
                Seats = 4,
                VehicleStatus = VehicleStatus.Free,
                DriverId = 1,
                CarrierId = 1,
                Picture = null,
                Longitude = 10,
                Latitude = 10,
                RatingAvg = 1,
                TotalRides = 1,
            };

            var vehicleId = await _myShuttleClient.VehiclesService.PostAsync(newVehicle);

            //Assert
            vehicleId.Should().BeGreaterThan(0);

            var newVehicleCount = await _myShuttleClient.VehiclesService.GetCountAsync(string.Empty);

            newVehicleCount.Should().Be(actualVehicleCount + 1);

            var currentVehicle = await _myShuttleClient.VehiclesService.GetAsync(vehicleId);

            //assert
            currentVehicle.Should().NotBeNull();

            currentVehicle.LicensePlate = Guid.NewGuid().ToString();

            await _myShuttleClient.VehiclesService.PutAsync(currentVehicle);

            var updateDriver = await _myShuttleClient.VehiclesService.GetAsync(vehicleId);

            updateDriver.Should().NotBeNull();
            updateDriver.LicensePlate.Should().Be(currentVehicle.LicensePlate);

            await _myShuttleClient.VehiclesService.DeleteAsync(vehicleId);
            newVehicleCount = await _myShuttleClient.VehiclesService.GetCountAsync(string.Empty);
            newVehicleCount.Should().Be(actualVehicleCount);

        }

        [TestMethod]
        public async Task VehiclesService_Test_Call_GetByPrice_Should_Return_Values()
        {
            var latitude = 10;
            var longitude = 10;

            var results = await _myShuttleClient.VehiclesService.GetByPriceAsync(latitude, longitude, 1);

            results.Should().NotBeNull();
            results.Count().Should().Be(1);
        }

        [TestMethod]
        public async Task VehiclesService_Test_Call_GetByDistance_Should_Return_Values()
        {
            var latitude = 10;
            var longitude = 10;

            var results = await _myShuttleClient.VehiclesService.GetByDistanceAsync(latitude, longitude, 1);

            results.Should().NotBeNull();
            results.Count().Should().Be(1);
        }
    }

}