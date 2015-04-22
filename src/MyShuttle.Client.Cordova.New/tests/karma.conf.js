module.exports = function (config) {
    config.set({
        basePath: './',
        frameworks: ['jasmine'],
        files: [
            'phantomBindPolyfill.js',
            '../www/libs/jquery-win8/jquery-1.8.2-win8-1.0.js',
            '../bower_components/angular/angular.js',
            '../bower_components/angular-mocks/angular-mocks.js',
            '../bower_components/angular-route/angular-route.js',
            '../bower_components/bootstrap/dist/js/bootstrap.js',
            '../bower_components/angular-bootstrap/ui-bootstrap-tpls.js',
            '../bower_components/moment/moment.js',
            '../bower_components/angular-moment/angular-moment.js',
            '../bower_components/signalr/jquery.signalR.js',

            '../www/scripts/appBundleGulp.js',

            '**/*.spec.js'
        ],
        browsers: ['PhantomJS'],
        singleRun: true,
        plugins: [
            'karma-jasmine',
            'karma-phantomjs-launcher'
        ]
    });
};