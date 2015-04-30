/// <reference path="../core.ts" />

module MyShuttle.Core {
    export class Vehicle {
        VehicleId: number;
        CarrierId: number;
        DriverId: number;
        Rate: number;
    }

    export class Coordinate {
        latitude: number;
        longitude: number;

        constructor(latitude: number, longitude: number) {
            this.latitude = latitude;
            this.longitude = longitude;
        }
    }

    export class SettingsService {
        public defaultEmployeeEmail: string;
        public vehicle: Vehicle;
        public startRideLocation: Coordinate;
        public endRideLocation: Coordinate;
        public rideDistance: number;
        public rideAddress: string;
        public bingMapsKey: string;
        public mobileServiceKey: string;
        public gcmSenderId: string;
        public realTimeNotificationsServerUrl: string;

        constructor(private storageService: MyShuttle.Core.StorageService) {
            this.defaultEmployeeEmail = 'jaysch@microsoft.com'; //Note - The azure mobile service looks up the employee by their email address

            this.vehicle = new Vehicle();
            this.vehicle.VehicleId = 5;
            this.vehicle.DriverId = 5;
            this.vehicle.CarrierId = 1;
            this.vehicle.Rate = 3;

            this.startRideLocation = new Coordinate(41.85026, -87.618894); //Ignite conference center
            this.endRideLocation = new Coordinate(41.9474536, -87.6561341); //Wrigley Field
            this.rideDistance = 8.5;
            this.rideAddress = 'Wrigley Field, 1060 W Addison St. Chicago, IL 60613';

            this.bingMapsKey = 'AowwaZssHfABfk67j7II30OYz2E4PF2qYsX3kSDLjokOyDLFR3HBozSlZY9gNb6e';
            this.mobileServiceKey = 'RgNJIxnKylcQcvXiCIsFWqACToAntZ27';
            this.gcmSenderId = '966619194956';
            this.realTimeNotificationsServerUrl = 'http://myshuttlebiz1ignitenew-rc.azurewebsites.net/';

            return this;
        }

        //This mobile service is used purely for data - it is NOT used for push notifications
        public getMobileServiceUrl():string {
            return this.storageService.getValue('serviceUrl', 'https://myshuttlemobileserviceignite1-rc.azure-mobile.net/');
        }
    }

    angularModule.factory('settingsService', (storageService) => {
        return new MyShuttle.Core.SettingsService(storageService);
    });
}