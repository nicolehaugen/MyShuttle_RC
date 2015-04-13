using MyShuttle.Client.Core.Infrastructure;
using MyShuttle.Client.Core.ServiceAgents;
using MyShuttle.Client.Core.Tests.ServiceAgents.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShuttle.Client.Core.DocumentResponse;
using System;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace MyShuttle.Client.Core.Tests.ServiceAgents
{
    [TestClass]
    public class CarriersServiceTests
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
        public async Task CarriersService_Test_Call_Get_Carrier_And_Update_it()
        {
            var currentCarrier = await _myShuttleClient.CarriersService.GetAsync();
            currentCarrier.Should().NotBeNull();

            currentCarrier.Name = Guid.NewGuid().ToString();

            await _myShuttleClient.CarriersService.PutAsync(currentCarrier);

            var updateCarrier = await _myShuttleClient.CarriersService.GetAsync();

            updateCarrier.Should().NotBeNull();
            updateCarrier.Name.Should().Be(currentCarrier.Name);
        }


        [TestMethod]
        public async Task CarriersService_Test_Call_All_CRUD_Methods_ShouldWork()
        {
            // Arrange

            // Act
            var newCarrier = new Carrier()
            {

            };

            var carrierId = await _myShuttleClient.CarriersService.PostAsync(newCarrier);

            //Assert
            carrierId.Should().BeGreaterThan(0);


        }
    }

}
