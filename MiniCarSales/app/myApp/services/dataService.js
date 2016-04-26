(function () {

    var injectParams = ['config', 'backendService', 'backendBreezeService'];

    var dataService = function (config, backendService, backendBreezeService) {
        return (config.useBreeze) ? backendBreezeService : backendService;
    };

    dataService.$inject = injectParams;

    angular.module('myApp').factory('dataService', dataService);

}());

