// reviewListController.js 

(function (){
    "use strict";
    
    // getting existing module
    angular.module("app-review").controller("reviewListController", reviewListController);

    function reviewListController($http) {
        var vm = this;
        vm.passages = [];
        vm.errorMessage = ""; 
        vm.isBusy = true;

        $http.get("/api/passages/review") 
            .then(function (response) { 
                angular.copy(response.data, vm.passages);
            }, function (error) { 
                vm.errorMessage = "Failed to load data: " + error;
            })
            .finally(function(){
                vm.isBusy = false;
            });
    }
})();