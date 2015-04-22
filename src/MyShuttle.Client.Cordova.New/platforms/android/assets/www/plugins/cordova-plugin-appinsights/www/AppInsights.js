cordova.define("cordova-plugin-appinsights.AppInsights", function(require, exports, module) { 
/*
Copyright (c) Microsoft Open Technologies, Inc.  All Rights Reserved.
Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.
*/

/*global module*/

var appInsightsConfig = {
    // To override this stub there is a three ways
    // * pass --variable INSTRUMENTATION_KEY=<your_key> to `cordova plugin add` command
    //      - THIS DOESN'T WORK YET DUE TO ISSUE WITH CORDOVA-LIB
    //
    // * add <preference name="instrumentation_key" value="<your_key>"> to config.xml at the root of project
    // * update this file manually
    instrumentationKey: "32bc0892-1d8f-4214-ac4e-d446a3e3dce8",
    // Need to specify this explicitly, because default value doesn't provide URL scheme
    endpointUrl: "https://dc.services.visualstudio.com/v2/track"
};

module.exports = appInsightsConfig;

});
