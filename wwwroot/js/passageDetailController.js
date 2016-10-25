// passageDetailController.js 

(function (){
    "use strict";
    
    // getting existing module
    angular.module("app-passages").controller("passageDetailController", passageDetailController);

    function passageDetailController($routeParams, $http) {
        var vm = this;

        vm.title = $routeParams.title;
        vm.passage = {};
        vm.errorMessage = ""; 
        vm.isBusy = true;

        $http.get(`/api/passages/detail/${vm.id}`) // returns a promise
            .then(function (response) { // success
                angular.copy(response.data, vm.passage);
            }, function (error) {  // failure 
                vm.errorMessage = "Failed to load data: " + error;
            })
            .finally(function(){
                vm.isBusy = false;
            });
    }
})();