(function () {

    var app = angular.module('myApp',
        ['ngRoute', 'ngAnimate', 'wc.directives', 'ui.bootstrap', 'breeze.angular']);

    app.config(['$routeProvider', function ($routeProvider) {
        var viewBase = '/app/myApp/views/';

        $routeProvider
            .when('/login/:redirect*?', {
                controller: 'LoginController',
                templateUrl: viewBase + 'login.html',
                controllerAs: 'vm'
            })
            .when('/vehicles', {
                controller: 'VehiclesController',
                templateUrl: viewBase + 'vehicles/vehicles.html',
                controllerAs: 'vm',
                secure: true //This route requires an authenticated user
            })
            .when('/vehicledit/:vehicleId', {
                controller: 'VehicleEditController',
                templateUrl: viewBase + 'vehicles/vehicleEdit.html',
                controllerAs: 'vm',
                secure: true //This route requires an authenticated user
            })
            .when('/enquiries', {
                controller: 'EnquiriesController',
                templateUrl: viewBase + 'enquiries/enquiries.html',
                controllerAs: 'vm',
                secure: true //This route requires an authenticated user
            })
            .when('/enquiryAdd/:vehicleId', {
                controller: 'EnquiryAddController',
                templateUrl: viewBase + 'enquiries/enquiryAdd.html',
                controllerAs: 'vm',
                secure: true //This route requires an authenticated user
            })
            .when('/search', {
                controller: 'SearchController',
                templateUrl: viewBase + 'vehicles/search.html',
                controllerAs: 'vm'                
            })
            .otherwise({ redirectTo: '/search' });
    }]);

    app.run(['$rootScope', '$location', 'authService',
        function ($rootScope, $location, authService) {
            
            //Client-side security. Server-side framework MUST add it's 
            //own security as well since client-based security is easily hacked
            $rootScope.$on("$routeChangeStart", function (event, next, current) {
                if (next && next.$$route && next.$$route.secure) {
                    if (!authService.user.isAuthenticated) {
                        $rootScope.$evalAsync(function () {
                            authService.redirectToLogin();
                        });
                    }
                }
            });

    }]);

}());

