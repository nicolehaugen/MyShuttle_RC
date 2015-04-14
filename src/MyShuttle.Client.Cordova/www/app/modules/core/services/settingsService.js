var MyShuttle;
(function (MyShuttle) {
    (function (Core) {
        var Vehicle = (function () {
            function Vehicle() {
            }
            return Vehicle;
        })();
        Core.Vehicle = Vehicle;

        var Coordinate = (function () {
            function Coordinate(latitude, longitude) {
                this.latitude = latitude;
                this.longitude = longitude;
            }
            return Coordinate;
        })();
        Core.Coordinate = Coordinate;

        var SettingsService = (function () {
            function SettingsService(storageService) {
                this.storageService = storageService;
                this.getMobileServiceUrl = function () {
                    return this.storageService.getValue('serviceUrl', 'http://localhost:35256/');
                };
                this.defaultEmployeeEmail = 'amanda@microsoft.com';

                this.vehicle = new Vehicle();
                this.vehicle.VehicleId = 5;
                this.vehicle.DriverId = 5;
                this.vehicle.CarrierId = 1;
                this.vehicle.Rate = 3;

                var coord = new Coordinate(40.72109533886229, -74.006596745813);
                this.startRideLocation = coord;
                this.endRideLocation = coord;
                this.rideDistance = 0.1;
                this.rideAddress = '50 Varick St, New York, NY 10012';

                this.bingMapsKey = 'AiOp5RxYtMGivuaQLXhKiGK4m2xBNk2WRXHWHPbXCVrCHvX2_ozTtMZPxtgO4QRO';
                this.mobileServiceKey = 'kJKvXlpCqfBWTzdKzDlSrYFJaYksny46';
                this.gcmSenderId = '615185909031';
                this.realTimeNotificationsServerUrl = 'http://myshuttle.azurewebsites.net/web/';

                return this;
            }
            return SettingsService;
        })();
        Core.SettingsService = SettingsService;

        Core.angularModule.factory('settingsService', function (storageService) {
            return new MyShuttle.Core.SettingsService(storageService);
        });
    })(MyShuttle.Core || (MyShuttle.Core = {}));
    var Core = MyShuttle.Core;
})(MyShuttle || (MyShuttle = {}));
//# sourceMappingURL=settingsService.js.map
