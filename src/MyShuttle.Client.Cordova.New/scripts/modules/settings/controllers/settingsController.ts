/// <reference path="../../core/core.ts" />
/// <reference path="../settings.ts" />
declare function refreshTodoItems();

module MyShuttle.Settings {
    interface ISettingsControllerScope extends ng.IScope {
        serviceUrl: string;
        saveSettings(): void;
    }

    angularModule.controller('SettingsController', function (
        $scope: ISettingsControllerScope,
        settingsService: MyShuttle.Core.SettingsService,
        storageService: MyShuttle.Core.StorageService,
        messengerService: MyShuttle.Core.MessengerService,
        navigationService: MyShuttle.Core.NavigationService) {

        $scope.saveSettings = function () {
            storageService.setValue('serviceUrl', $scope.serviceUrl);
            messengerService.send(messengerService.messageTypes.settingsChanged);
            navigationService.navigateBack();
        };

        var init = function () {
            messengerService.send(messengerService.messageTypes.showNavigateBackBtn, { jumpToMainPage: false });
            $scope.serviceUrl = settingsService.getMobileServiceUrl();
            $scope.$on('$locationChangeStart', function (event) {
                messengerService.send(messengerService.messageTypes.hideNavigateBackBtn);
            });

            mobileServiceClient = new WindowsAzure.MobileServiceClient(
                "https://myshuttlepushnotificationmobileservice.azure-mobile.net/",
                "muLzKtUgItkcGCSRkvxovHKaygpRSS90");
            // Define the PushPlugin.
            pushNotification = window.plugins.pushNotification;

            //TODO: remove GCM registration for the demo
            // Platform-specific registrations.
            if (device.platform == 'android' || device.platform == 'Android') {
                // Register with GCM for Android apps.
                pushNotification.register(onSuccess, error,
                    {
                        "senderID": GCM_SENDER_ID,
                        "ecb": "onGcmNotification"
                    });
            }
            else if (device.platform === 'Win32NT') {
                pushNotification.register(
                    this.channelHandler,
                    this.error,
                    {
                        'channelName': 'MyPushChannel',
                        'ecb': 'onNotificationWP8',
                        'uccb': 'channelHandler',
                        'errcb': 'errorHandler'
                    });
            } 

            //TODo: Need to move to when after request received
            var table = mobileServiceClient.getTable('NotificationTable');
            table.insert({ text: "Called from Cordova", complete: false })
        }

        init();
    });
}

function onSuccess(e) {
    var test = "Nicole";
}

function error(error) {
    console.log('Error: ' + error);
}

   // Windows Phone channel handler.
function channelHandler(result: any) {

    console.log("channelHandler called");
    if (result.uri !== '') {
        if (this.mobileServiceClient) {
            var hub = new NotificationHub(this.mobileServiceClient);

            var template = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<wp:Notification xmlns:wp=\"WPNotification\">" +
                "<wp:Toast>" +
                "<wp:Text1>Request received</wp:Text1>" +
                "<employeeId>$(employeeId)</employeeId>" +
                "<latitude>$(latitude)</latitude>" +
                "<longitude>$(longitude)</longitude>" +
                "</wp:Toast>" +
                "</wp:Notification>";

            //hub.mpns.register(result.uri, this.tagsToRegister, 'myTemplate', template)
            hub.mpns.register(result.uri)
                .done(function () {
                console.log('Registered with hub.');
            }).fail(function (error) {
                console.log('Failed registering with hub: ' + error);
            });
        }
    }
    else {
        console.log('Channel URI could not be obtained.');
    }
}


var GCM_SENDER_ID = '966619194956'; // Replace with your own ID.
var mobileServiceClient;
var pushNotification;
// Create the Azure client register for notifications.
document.addEventListener('deviceready', function () {
});

// Handle a GCM notification.
function onGcmNotification(e) {
    switch (e.event) {
        case 'registered':
            // Handle the registration.
            if (e.regid.length > 0) {
                console.log("gcm id " + e.regid);
                if (mobileServiceClient) {
                    // Template registration.
                    var template = "{ \"data\" : {\"message\":\"$(message)\"}}";
                    // Register for notifications.
                    mobileServiceClient.push.gcm.registerTemplate(e.regid,
                        "myTemplate", template, null)
                        .done(function () {
                        alert('Registered template with Azure!');
                    }).fail(function (error) {
                        alert('Failed registering with Azure: ' + error);
                    });
                }
            }
            break;
        case 'message':
            if (e.foreground) {
                // Handle the received notification when the app is running
                // and display the alert message.
                alert(e.payload.message);
                // Reload the items list.
                refreshTodoItems();
            }
            break;
        case 'error':
            alert('Google Cloud Messaging error: ' + e.message);
            break;
        default:
            alert('An unknown GCM event has occurred');
            break;
    }
}
