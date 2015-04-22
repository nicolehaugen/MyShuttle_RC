cordova.define('cordova/plugin_list', function(require, exports, module) {
module.exports = [
    {
        "file": "plugins/org.apache.cordova.vibration/www/vibration.js",
        "id": "org.apache.cordova.vibration.notification",
        "merges": [
            "navigator.notification",
            "navigator"
        ]
    },
    {
        "file": "plugins/com.phonegap.plugins.PushPlugin/www/PushNotification.js",
        "id": "com.phonegap.plugins.PushPlugin.PushNotification",
        "clobbers": [
            "PushNotification"
        ]
    },
    {
        "file": "plugins/cordova-plugin-appinsights/www/AppInsights.js",
        "id": "cordova-plugin-appinsights.AppInsights",
        "clobbers": [
            "window.appInsights.config"
        ]
    },
    {
        "file": "plugins/cordova-plugin-appinsights/www/AppInsights.lib.js",
        "id": "cordova-plugin-appinsights.AppInsightsLibrary",
        "runs": true
    },
    {
        "file": "plugins/cordova-plugin-dialogs/www/notification.js",
        "id": "cordova-plugin-dialogs.notification",
        "merges": [
            "navigator.notification"
        ]
    },
    {
        "file": "plugins/cordova-plugin-dialogs/www/android/notification.js",
        "id": "cordova-plugin-dialogs.notification_android",
        "merges": [
            "navigator.notification"
        ]
    },
    {
        "file": "plugins/com.microsoft.azure-mobile-services/www/MobileServices.Web.min.js",
        "id": "com.microsoft.azure-mobile-services.AzureMobileServices",
        "runs": true
    },
    {
        "file": "plugins/org.apache.cordova.device/www/device.js",
        "id": "org.apache.cordova.device.device",
        "clobbers": [
            "device"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.inappbrowser/www/inappbrowser.js",
        "id": "org.apache.cordova.inappbrowser.inappbrowser",
        "clobbers": [
            "window.open"
        ]
    }
];
module.exports.metadata = 
// TOP OF METADATA
{
    "org.apache.cordova.vibration": "0.3.13",
    "com.phonegap.plugins.PushPlugin": "2.4.0",
    "cordova-plugin-appinsights": "0.0.2",
    "cordova-plugin-dialogs": "1.0.1-dev",
    "com.microsoft.azure-mobile-services": "1.2.7",
    "org.apache.cordova.device": "0.3.0",
    "org.apache.cordova.inappbrowser": "0.6.0"
}
// BOTTOM OF METADATA
});