(function () {

    var injectParams = ['$http', '$q'];

    var customersFactory = function ($http, $q) {
        var serviceBase = '/api/dataservice/',
            factory = {};

        factory.getMakes = function () {            
            return $http.get(serviceBase + 'makes').then(
                function (results) {
                    return results.data;
                });
        }

        factory.getModels = function (id) {
            return $http.get(serviceBase + 'models/?makeId=' + id).then(
                function (results) {
                    return results.data;
                });
        }

        factory.getYears = function () {
            return $http.get(serviceBase + 'years').then(
                function (results) {
                    return results.data;
                });
        }

        factory.getVehicles = function (makeId, modelId, pageIndex, pageSize) {
            var servicePath =serviceBase + "vehicles/";
            var resource = buildPagingUri(pageIndex, pageSize);

            if (makeId != null && makeId > 0) resource += "&makeId=" + makeId;

            if (modelId != null && modelId > 0) resource += "&modelId=" + modelId;

            return $http.get(servicePath + resource).then(function (response) {
                var data = response.data;
                return {
                    totalRecords: parseInt(response.headers('X-InlineCount')),
                    results: data
                };
            });
        };

        factory.getVehicle = function (vehicleId) {
            var servicePath = serviceBase + "vehicle/";

            return $http.get(servicePath + vehicleId).then(function (response) {
                var data = response.data;
                return {                    
                    results: data
                };
            });
        };

        factory.addEnquiry = function (enquiry) {
            return $http.post(serviceBase + "postEnquiry", enquiry).then(function (results) {
                return results.data;
            })
        };

        factory.saveVehicle = function (vehicle) {
            return $http.post(serviceBase + "postVehicle", vehicle).then(function (results) {
                return results.data;
            })
        };

        factory.deleteVehicle = function (vehicleId) {
            return $http.delete(serviceBase + "deleteVehicle/" + vehicleId).then(function (results) {
                return results.data;
            })
        };

        factory.fileUploadService = function (file, description) {
                var formData = new FormData();
                formData.append("file", file);
                //We can send more data to server using append         
                formData.append("description", description);

                var defer = $q.defer();
                $http.post(serviceBase + "UploadFile", formData,
                    {
                        withCredentials: true,
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                    })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

                return defer.promise;
        };

        factory.getEnquiries = function () {
            return $http.get(serviceBase + 'Enquiries').then(
                function (results) {
                    return results.data;
                });
        }

        factory.getCustomers = function (pageIndex, pageSize) {
            return getPagedResource('customers', pageIndex, pageSize);
        };

        factory.getCustomersSummary = function (pageIndex, pageSize) {
            return getPagedResource('customersSummary', pageIndex, pageSize);
        };

        factory.getStates = function () {
            return $http.get(serviceBase + 'states').then(
                function (results) {
                    return results.data;
                });
        };

        factory.checkUniqueValue = function (id, property, value) {
            if (!id) id = 0;
            return $http.get(serviceBase + 'checkUnique/' + id + '?property=' + property + '&value=' + escape(value)).then(
                function (results) {
                    return results.data.status;
                });
        };

        factory.insertCustomer = function (customer) {
            return $http.post(serviceBase + 'postCustomer', customer).then(function (results) {
                customer.id = results.data.id;
                return results.data;
            });
        };

        factory.newCustomer = function () {
            return $q.when({id: 0});
        };

        factory.updateCustomer = function (customer) {
            return $http.put(serviceBase + 'putCustomer/' + customer.id, customer).then(function (status) {
                return status.data;
            });
        };

        factory.deleteCustomer = function (id) {
            return $http.delete(serviceBase + 'deleteCustomer/' + id).then(function (status) {
                return status.data;
            });
        };

        factory.getCustomer = function (id) {
            //then does not unwrap data so must go through .data property
            //success unwraps data automatically (no need to call .data property)
            return $http.get(serviceBase + 'customerById/' + id).then(function (results) {
                extendCustomers([results.data]);
                return results.data;
            });
        };

        function extendCustomers(customers) {
            var custsLen = customers.length;
            //Iterate through customers
            for (var i = 0; i < custsLen; i++) {
                var cust = customers[i];
                if (!cust.orders) continue;

                var ordersLen = cust.orders.length;
                for (var j = 0; j < ordersLen; j++) {
                    var order = cust.orders[j];
                    order.orderTotal = order.quantity * order.price;
                }
                cust.ordersTotal = ordersTotal(cust);
            }
        }

        function getPagedResource(baseResource, pageIndex, pageSize) {
            var resource = baseResource;
            resource += (arguments.length == 3) ? buildPagingUri(pageIndex, pageSize) : '';
            return $http.get(serviceBase + resource).then(function (response) {
                var custs = response.data;
                extendCustomers(custs);
                return {
                    totalRecords: parseInt(response.headers('X-InlineCount')),
                    results: custs
                };
            });
        }

        function buildPagingUri(pageIndex, pageSize) {
            var uri = '?top=' + pageSize + '&skip=' + (pageIndex * pageSize);
            return uri;
        }

        // is this still used???
        function orderTotal(order) {
            return order.quantity * order.price;
        };

        function ordersTotal(customer) {
            var total = 0;
            var orders = customer.orders;
            var count = orders.length;

            for (var i = 0; i < count; i++) {
                total += orders[i].orderTotal;
            }
            return total;
        };

        return factory;
    };

    customersFactory.$inject = injectParams;

    angular.module('myApp').factory('backendService', customersFactory);

}());