/// <reference path="../rides.ts" />

module MyShuttle.Rides {
    interface IHomeControllerScope extends ng.IScope {
        counter: number;
    }

    angularModule.controller('HomeController', function (
        $scope: IHomeControllerScope,
        pushNotificationsService: MyShuttle.Rides.IPushNotificationsService,
        settingsService: MyShuttle.Core.SettingsService,
        dataService: MyShuttle.Rides.DataService,
        messengerService: MyShuttle.Core.MessengerService,
        modalService: MyShuttle.Rides.ModalService,
        navigationService:  MyShuttle.Core.NavigationService) {

        var timeRequest = moment();

        //TODO: Need to replace this with Jay's employee id
        var employeeId = '90a0d28b-4342-40d5-92b6-af2d0ea7630b';
        var coord = new MyShuttle.Core.Coordinate(41.85026, -87.618894);

        messengerService.send(messengerService.messageTypes.startLoading);

        dataService.getEmployee(employeeId).done(function (results) {
            messengerService.send(messengerService.messageTypes.endLoading);
            var employee = results[0];
            modalService.showRideRequest(employee, coord).then(function (result) {
                if (result) {
                    var params = { employee: employee, position: coord, timeRequest: timeRequest };
                    navigationService.navigateTo('service', params);
                }
                else {
                    alert ('employee not found');
                }
            });
        });

        var init = function () {
            //pushNotificationsService.initPushNotifications(function (data: MyShuttle.Rides.INotificationData) {
            //    timeRequest = moment();
            //    showRequest(data);
            //});
        }

        init();
    });
}
