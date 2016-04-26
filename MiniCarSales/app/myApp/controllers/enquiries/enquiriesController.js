(function () {

    var injectParams = ['$location', '$filter', '$window',
                        '$timeout', 'authService', 'dataService', 'modalService'];

    var EnquiriesController = function ($location, $filter, $window,
        $timeout, authService, dataService, modalService) {

        var vm = this;

        vm.enquiries = [];
        vm.cardAnimationClass = '.card-animation';

        //paging
        vm.totalRecords = 0;
        vm.pageSize = 10;
        vm.currentPage = 1;

        vm.pageChanged = function (page) {
            vm.currentPage = page;
            //getCustomersSummary();
        };        

        function init() {
            dataService.getEnquiries()
            .then(function (data) {
                vm.enquiries = data;
            });
        }

        init();
    };

    EnquiriesController.$inject = injectParams;

    angular.module('myApp').controller('EnquiriesController', EnquiriesController);

}());