/// <reference path="../rides.ts" />

var dataServiceHandler;
var messengerServiceHandler;
var navigationServiceHandler;
var modalServiceHandler;

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
        navigationService: MyShuttle.Core.NavigationService) {

        var init = function () {
            dataServiceHandler = dataService;
            messengerServiceHandler = messengerService;
            navigationServiceHandler = navigationService;
            modalServiceHandler = modalService;

            mobileServiceClient = new WindowsAzure.MobileServiceClient(
                "https://myshuttlepushnotificationmobileservice.azure-mobile.net/",
                "muLzKtUgItkcGCSRkvxovHKaygpRSS90");

            // Define the PushPlugin
            pushNotification = window.plugins.pushNotification;

            //Register this app to receive android notifications
            if (device.platform == 'android' || device.platform == 'Android') {
                pushNotification.register(onSuccess, error,
                    {
                        "senderID": GCM_SENDER_ID,
                        "ecb": "onGcmNotificationTest"
                    });
            }
        }

        init();
    });
}

var GCM_SENDER_ID = '966619194956';  //This is the Google project id found in the Google developer console

// Handle a GCM notification.
function onGcmNotificationTest(e) {
    switch (e.event) {
        case 'registered':
            // Handle the registration.
            if (e.regid.length > 0) {
                console.log("gcm id: " + e.regid);
                if (mobileServiceClient) {
                    // Template registration
                    var template = "{ \"data\" : {\"message\":\"$(message)\"}}";
                    // Register for notifications
                    mobileServiceClient.push.gcm.registerTemplate(e.regid,
                        "myTemplate", template, null)
                        .done(function () {
                        console.log('Registered template with Azure!');
                    }).fail(function (error) {
                        console.log('Failed registering with Azure: ' + error);
                    });
                }
            }
            break;
        case 'message':
            if (e.foreground) {
                var timeRequest = moment();

                //This is hard-coded to Jay's employee id
                //var employeeId = '90a0d28b-4342-40d5-92b6-af2d0ea7630b';

                //Get the employee id from the received push notification
                alert('message ' + e.payload.message);
                var employeeId = e.payload.message;
                var coord = new MyShuttle.Core.Coordinate(41.85026, -87.618894);

                messengerServiceHandler.send(messengerServiceHandler.messageTypes.startLoading);

                //Shows the dialog with the the employee's (e.g. customer's) information
                dataServiceHandler.getEmployee(employeeId).done(function (results) {
                    messengerServiceHandler.send(messengerServiceHandler.messageTypes.endLoading);
                    var employee = results[0];
                    modalServiceHandler.showRideRequest(employee, coord).then(function (result) {
                        if (result) {
                            var params = { employee: employee, position: coord, timeRequest: timeRequest };
                            navigationServiceHandler.navigateTo('service', params);
                        }
                        else {
                            console.log('Employee not found');
                        }
                    });
                });
            }
            break;
        case 'error':
            console.log('Google Cloud Messaging error: ' + e.message);
            break;
        default:
            console.log('An unknown GCM event has occurred');
            break;
    }
}


