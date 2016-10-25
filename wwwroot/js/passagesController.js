// passagesController.js 

(function (){
    "use strict";
    
    // getting existing module
    angular.module("app-passages").controller("passagesController", passagesController);

    function passagesController($http, $scope, $location) {
        var vm = this;
        vm.passages = [];
        vm.errorMessage = ""; 
        vm.isBusy = true;

        $http.get("/api/passages") // returns a promise
            .then(function (response) { // success
                angular.copy(response.data, vm.passages);
            }, function (error) {  // failure 
                vm.errorMessage = "Failed to load data: " + error;
            })
            .finally(function(){
                vm.isBusy = false;
            });
    }
})();