// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397705
// To debug code on page load in Ripple or on Android devices/emulators: launch your app, set breakpoints, 
// and then run "window.location.reload()" in the JavaScript Console.
declare var appInsights: any

(function () {
    "use strict";
        
    //Start by defining the main module and adding the module dependencies
    angular.module(ApplicationConfiguration.applicationName, ApplicationConfiguration.applicationModuleVendorDependencies);

    document.addEventListener('deviceready', onDeviceReady, false);

    function onDeviceReady() {
        
        // Track app is started
        appInsights.trackPageView();
        var eventData = { Timestamp: new Date() };
        appInsights.trackEvent('deviceready', eventData);

        // Handle the Cordova pause and resume events
        document.addEventListener('pause', onPause, false);
        document.addEventListener('resume', onResume, false);

        // Then init the app
        angular.bootstrap(document, [ApplicationConfiguration.applicationName]);
    }

    function onPause() {
        // TODO: This application has been suspended. Save application state here.
    }

    function onResume() {
        // TODO: This application has been reactivated. Restore application state here.
    }

})();
