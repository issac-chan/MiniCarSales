(function () {

    var injectParams = ['$location', '$filter', '$window',
                        '$timeout', 'authService', 'dataService', 'modalService'];

    var SearchController = function ($location, $filter, $window,
        $timeout, authService, dataService, modalService) {

        var vm = this;

        vm.Vehicles = [];
        vm.filteredVehicles = [];
        vm.filteredCount = 0;
        vm.orderby = 'lastName';
        vm.reverse = false;
        vm.searchText = null;
        vm.cardAnimationClass = '.card-animation';
        vm.makes = [];
        vm.makeId = 0;
        vm.models = [];
        vm.modelId = 0;
        vm.enquiryName = [];
        vm.enquiryEmail = [];
        vm.enquiryPhone = [];
        vm.enquiryComment = [];
        vm.enquiryResult = [];

        //paging
        vm.totalRecords = 0;
        vm.pageSize = 10;
        vm.currentPage = 1;

        vm.pageChanged = function (page) {
            vm.currentPage = page;
            //getCustomersSummary();
        };      

        vm.loadModels = function () {

            if (vm.makeId == null)
            {
                vm.models = [];

                return true;
            }

            return dataService.getModels(vm.makeId).then(function (models) {
                vm.models = models;
            }, processError);
        };

        vm.searchVehicle = function () {
            dataService.getVehicles(vm.makeId, vm.modelId, vm.currentPage - 1, vm.pageSize)
            .then(function (data) {                
                vm.totalRecords = data.totalRecords;
                vm.filteredVehicles = data.results;
                $timeout(function () {
                    vm.cardAnimationClass = ''; //Turn off animation since it won't keep up with filtering
                }, 1000);
            }, function (error) {
                $window.alert('Sorry, an error occurred: ' + error.data.message);
            });
        };

        vm.submitEnquiry = function (vehicleId) {
            var contactName = vm.enquiryName[vehicleId];
            
            var enquiry = { Name: vm.enquiryName[vehicleId], Email: vm.enquiryEmail[vehicleId], Phone: vm.enquiryPhone[vehicleId], Comment : vm.enquiryComment[vehicleId], VehicleId: vehicleId};

            dataService.addEnquiry(enquiry).then(function(data){
                if(data)
                {
                    vm.enquiryResult[vehicleId] = data;
                }

                $timeout(function () {
                    vm.cardAnimationClass = ''; //Turn off animation since it won't keep up with filtering
                }, 1000);
            }, function (error) {
                $window.alert('Sorry, an error occurred: ' + error.data.message);
            });
        };

        function init() {
            loadMakes();
        }     

        function loadMakes() {
            return dataService.getMakes().then(function (makes) {
                vm.makes = makes;
            }, processError);
        }

        function processSuccess() {
            $scope.editForm.$dirty = false;
            vm.updateStatus = true;
            vm.title = 'Edit';
            vm.buttonText = 'Update';
            startTimer();
        }

        function processError(error) {
            vm.errorMessage = error.message;
            startTimer();
        }

        function startTimer() {
            timer = $timeout(function () {
                $timeout.cancel(timer);
                vm.errorMessage = '';
                vm.updateStatus = false;
            }, 3000);
        }

        init();
    };

    SearchController.$inject = injectParams;

    angular.module('myApp').controller('SearchController', SearchController);

}());