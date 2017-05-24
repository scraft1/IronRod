// reviewTopicsListController.js 

(function (){
    "use strict";
    
    // getting existing module
    angular.module("app-review").controller("reviewTopicsListController", reviewTopicsListController);

    function reviewTopicsListController($http) {
        var vm = this;
        vm.topics = [];
        vm.errorMessage = ""; 
        vm.isBusy = true;

        $http.get("/api/review/topics") 
            .then(function (response) { 
                angular.copy(response.data, vm.topics);
            }, function (error) { 
                vm.errorMessage = "Failed to load topics: " + error;
            })
            .finally(function(){
                vm.isBusy = false;
            });
    }

})();