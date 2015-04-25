/// <reference path="../rides.ts" />

declare var NotificationHub: any;

module MyShuttle.Rides {

    export interface IPushNotificationsService {
        initPushNotifications(callback: any): void;
        notifyApprovedRequest(employeeId: string): ng.IHttpPromise<any>;
        notifyRejectedRequest(employeeId: string): ng.IHttpPromise<any>;
        notifyVehicleArrived(employeeId: string): ng.IHttpPromise<any>
    }

    export interface INotificationData {
        employeeId: string;
        latitude: number;
        longitude: number;
    }

    class PushNotificationsService implements IPushNotificationsService {

        private mobileClient: Microsoft.WindowsAzure.MobileServiceClient;
        private tagsToRegister: string[];
        private notificationCallback: (data: INotificationData) => void;

        constructor(private $http: ng.IHttpService,
            private $window: ng.IWindowService,
            private settingsService: MyShuttle.Core.SettingsService) {

            $window['onNotificationGCM'] = this.onNotificationGCM;
            $window['onNotificationWP8'] = this.onNotificationWP8;
            $window['onNotificationAPN'] = this.onNotificationAPN;
            $window['channelHandler'] = this.channelHandler;
            $window['errorHandler'] = this.errorHandler;

            return this;
        }

        initPushNotifications(callback: (data: INotificationData) => void) {
            // Only register notifications if not running in Ripple
            var emulated = this.$window['tinyHippos'] != undefined;
            if (emulated) return;

            var mobileServiceUrl: string = this.settingsService.getMobileServiceUrl();
            var zumoApplicationKey = this.settingsService.mobileServiceKey;
            this.tagsToRegister = ['VehicleRequested-' + this.settingsService.vehicle.DriverId];
            this.notificationCallback = callback;

            this.mobileClient = new WindowsAzure.MobileServiceClient(mobileServiceUrl, zumoApplicationKey);

            this.register();
        }

        notifyApprovedRequest(employeeId: string): ng.IHttpPromise<any> {
            return this.notify('NotifyApprovedRequest', employeeId);
        }

        notifyRejectedRequest(employeeId: string): ng.IHttpPromise<any> {
            return this.notify('NotifyRejectedRequest', employeeId);
        }

        notifyVehicleArrived(employeeId: string): ng.IHttpPromise<any> {
            return this.notify('NotifyVehicleArrived', employeeId);
        }

        private register(): void {
            var pushNotification = window.plugins.pushNotification;

            if (device.platform === 'android' || device.platform === 'Android') {
                var gcmSenderId = this.settingsService.gcmSenderId;

                pushNotification.register(
                    this.successHandler,
                    this.errorHandler,
                    {
                        'senderID': gcmSenderId,
                        'ecb': 'onNotificationGCM'
                    });
            } else if (device.platform === 'Win32NT') {
                pushNotification.register(
                    this.channelHandler,
                    this.errorHandler,
                    {
                        'channelName': 'MyPushChannel',
                        'ecb': 'onNotificationWP8',
                        'uccb': 'channelHandler',
                        'errcb': 'errorHandler'
                    });
            } else if (device.platform === 'iOS') {
                pushNotification.register(
                    this.tokenHandler,
                    this.errorHandler,
                    {
                        'ecb': 'onNotificationAPN'
                    });
            }
        }

        private successHandler(registrationId: string): void {
            console.log('Callback success, result: ' + registrationId);
        }

        // Windows Phone channel handler.
        private channelHandler(result: any) {
            if (result.uri !== '') {
                if (this.mobileClient) {
                    var hub = new NotificationHub(this.mobileClient);

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

        // iOS token handler.
        private tokenHandler(result) {
            if (this.mobileClient) {
                var hub = new NotificationHub(this.mobileClient);

                var template = "{" +
                    "\"aps\" : {" +
                    "\"alert\":\"Request received\"" +
                    "}," +
                    "\"employeeId\":\"$(employeeId)\"," +
                    "\"latitude\":\"$(latitude)\"," +
                    "\"longitude\":\"$(longitude)\"" +
                    "}";

                hub.apns.register(result, this.tagsToRegister, 'myTemplate', template, null)
                    .done(function () {
                    console.log('Registered with hub.');
                }).fail(function (error) {
                    console.log('Failed registering with hub: ' + error);
                });
            }
        }

        private errorHandler(error: any): void {
            console.log('Error: ' + error);
        }

        private notify(method: string, employeeId: string): ng.IHttpPromise<any> {
            var mobileServiceUrl = this.settingsService.getMobileServiceUrl();
            var zumoApplicationKey = this.settingsService.mobileServiceKey;

            return this.$http.post(mobileServiceUrl + '/api/' + method,
                { employeeId: employeeId },
                {
                    headers: { 'X-ZUMO-APPLICATION': zumoApplicationKey }
                });
        }

        private onNotificationGCM(e) {
            switch (e.event) {
                case 'registered':
                    if (e.regid.length > 0) {
                        console.log('GCM identifier:  ' + e.regid);
                        if (this.mobileClient) {
                            var hub = new NotificationHub(this.mobileClient);

                            var template = "{ \"data\" : {" +
                                "\"title\":\"Request received\"," +
                                "\"message\":\"Touch to see the information\"," +
                                "\"employeeId\":\"$(employeeId)\"," +
                                "\"latitude\":\"$(latitude)\"," +
                                "\"longitude\":\"$(longitude)\"" +
                                "}}";

                            hub.gcm.register(e.regid, this.tagsToRegister, 'myTemplate', template).done(function () {
                                console.log('Registered with hub.');
                            }).fail(function (error) {
                                console.log('Failed registering with hub: ' + error);
                            });
                        }
                    }
                    break;
                case 'message':
                    if (e.foreground) {
                        this.runAudioNotification();
                    }
                    this.processMessage(e.payload);
                    break;
                case 'error':
                    console.log('GCM error: ' + e.message);
                    break;
                default:
                    console.log('An unknown GCM event has occurred.');
                    break;
            }
        }

        private onNotificationAPN(e) {
            if (e.alert) {
                this.runAudioNotification();
                this.processMessage(e);
            }
        }

        private onNotificationWP8(e) {
            if (e.type == 'toast' && e.jsonContent) {
                this.runAudioNotification();
                this.processMessage(e.jsonContent);
            }
        }

        private runAudioNotification() {
            navigator.notification.vibrate(1000);
            navigator.notification.beep(1);
        }

        private processMessage(message) {
            var data: INotificationData = {
                employeeId: message.employeeId,
                latitude: parseFloat(message.latitude),
                longitude: parseFloat(message.longitude)
            };
            this.notificationCallback(data);
        }
    }

    angularModule.factory('pushNotificationsService',($http: ng.IHttpService, $window: ng.IWindowService, settingsService: MyShuttle.Core.SettingsService) => {
        return new PushNotificationsService($http, $window, settingsService);
    });
}