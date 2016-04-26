(function () {

    var injectParams = ['$scope', '$location', '$routeParams', '$window',
                        '$timeout', 'config', 'dataService', 'modalService'];

    var VehicleEditController = function ($scope, $location, $routeParams, $window,
                                           $timeout, config, dataService, modalService) {

        var vm = this,
            vehicleId = ($routeParams.vehicleId) ? parseInt($routeParams.vehicleId) : 0,
            timer,
            onRouteChangeOff;

        vm.vehicle = {};
        vm.makes = [];
        vm.models = [];
        vm.years = [];
        vm.title = (vehicleId > 0) ? 'Edit' : 'Add';
        vm.buttonText = (vehicleId > 0) ? 'Update' : 'Add';
        vm.updateStatus = false;
        vm.errorMessage = '';
        $scope.Message = "";
        $scope.FileInvalidMessage = "";
        $scope.SelectedFileForUpload = null;
        $scope.FileDescription = "";
        $scope.IsFormSubmitted = false;
        $scope.IsFileValid = false;
        $scope.IsFormValid = false;

        function saveVehicle() {
            if ($scope.editForm.$valid) {

                dataService.saveVehicle(vm.vehicle)
                .then(function (data) {
                    $window.location.href = "#/vehicles";
                });
            }
        };

        $scope.ChechFileValid = function (file) {
            var isValid = false;
            if ($scope.SelectedFileForUpload != null) {
                if ((file.type == 'image/png' || file.type == 'image/jpeg' || file.type == 'image/gif') && file.size <= (512 * 1024)) {
                    $scope.FileInvalidMessage = "";
                    isValid = true;
                }
                else {
                    $scope.FileInvalidMessage = "Selected file is Invalid. (only file type png, jpeg and gif and 512 kb size allowed)";
                }
            }
            else {
                $scope.FileInvalidMessage = "Image required!";
            }
            $scope.IsFileValid = isValid;
        };

        //File Select event 
        $scope.selectFileforUpload = function (file) {
            $scope.SelectedFileForUpload = file[0];
        }
        //----------------------------------------------------------------------------------------

        //Save File
        $scope.SaveFile = function () {
            $scope.IsFormSubmitted = true;
            $scope.Message = "";
            $scope.ChechFileValid($scope.SelectedFileForUpload);
            if ($scope.editForm.$valid && $scope.IsFileValid) {
                dataService.fileUploadService($scope.SelectedFileForUpload, $scope.FileDescription).then(function (d) {
                    vm.vehicle.photo = d;
                    saveVehicle();
                }, function (e) {
                    alert(e);
                });
            }
            else {
                $scope.Message = "All the fields are required.";
            }
        };

        function init() {
            loadYears();
            loadMakes();
            retrieveVehcleData();

            onRouteChangeOff = $scope.$on('$locationChangeStart', routeChange);
        }

        init();

        function retrieveVehcleData() {
            if (vehicleId == null || vehicleId <= 0) return;

            dataService.getVehicle(vehicleId)
            .then(function (result) {
                vm.vehicle = result;
            });
        }

        function routeChange(event, newUrl, oldUrl) {
            //Navigate to newUrl if the form isn't dirty
            if (!vm.editForm || !vm.editForm.$dirty) return;

            var modalOptions = {
                closeButtonText: 'Cancel',
                actionButtonText: 'Ignore Changes',
                headerText: 'Unsaved Changes',
                bodyText: 'You have unsaved changes. Leave the page?'
            };

            modalService.showModal({}, modalOptions).then(function (result) {
                if (result === 'ok') {
                    onRouteChangeOff(); //Stop listening for location changes
                    $location.path($location.url(newUrl).hash()); //Go to page they're interested in
                }
            });

            event.preventDefault();
            return;
        }

        function loadMakes() {
            return dataService.getMakes().then(function (makes) {
                vm.makes = makes;
            }, processError);
        }

        vm.loadModels = function () {

            if (vm.vehicle.makeId == null) {
                vm.models = [];

                return true;
            }

            return dataService.getModels(vm.vehicle.makeId).then(function (models) {
                vm.models = models;
            }, processError);
        };

        vm.cancel = function () {
            $window.location.href = "#/vehicles";
        };

        function loadYears()
        {
            return dataService.getYears().then(function (years) {
                vm.years = years;
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
    };

    VehicleEditController.$inject = injectParams;

    angular.module('myApp').controller('VehicleEditController', VehicleEditController);

}());