/// <reference path="../rides.ts" />

module MyShuttle.Rides {
    interface IServiceControllerScope extends ng.IScope {
        employee: any;
        position: MyShuttle.Core.Coordinate;
        timeRequest: moment.Moment;
        notifyEmployee(): void;
    }

    angularModule.controller('ServiceController', function (
        $scope: IServiceControllerScope,
        pushNotificationsService: MyShuttle.Rides.IPushNotificationsService,
        navigationService: MyShuttle.Core.NavigationService,
        params) {

        $scope.notifyEmployee = function () {

            mobileServiceClient = new WindowsAzure.MobileServiceClient(
                "https://myshuttlepushnotificationmobileservice.azure-mobile.net/",
                "muLzKtUgItkcGCSRkvxovHKaygpRSS90");

            var table = mobileServiceClient.getTable('NotificationTable');
            table.insert({ text: "Vehicle has arrived", complete: false })
            navigationService.navigateTo('ride', $scope.employee);
        }

        var init = function () {
            if (!params) {
                navigationService.navigateTo('home');
            }
            var table = mobileServiceClient.getTable('NotificationTable');
            table.insert({ text: "Vehicle has been sent", complete: false })

            $scope.employee = params.employee;
            $scope.position = params.position;
            $scope.timeRequest = params.timeRequest;
        };

        init();
    });
}
