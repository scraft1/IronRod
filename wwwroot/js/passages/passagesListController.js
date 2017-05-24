// passagesListController.js 

(function (){
    "use strict";
    
    // getting existing module
    angular.module("app-passages").controller("passagesListController", passagesListController);

    function passagesListController($http) {
        var vm = this;
        vm.stats = {};
        vm.passages = [];
        vm.errorMessage = ""; 
        vm.isBusy = true;

        $http.get("/api/passages/stats") 
            .then(function (response) {
                angular.copy(response.data, vm.stats);
            }, function (error) { 
                vm.errorMessage = "Failed to load stats: " + error;
            });

        $http.get("/api/passages")
            .then(function (response) { 
                angular.copy(response.data, vm.passages);
                vm.handleNewDatePassed();
                vm.sortByReference();
            }, function (error) {  
                vm.errorMessage = "Failed to load passages: " + error;
            })
            .finally(function(){
                vm.isBusy = false;
            });

        vm.handleNewDatePassed = function(){
            for(var i = 0; i < vm.passages.length; i++){
                var p = vm.passages[i];
                if(p.level == 0){
                    p.datePassed = "--";
                }
            }
        }

        vm.sortByReference = function() {
            vm.passages.sort(function(a,b) {
                if(a.firstVerse < b.firstVerse) return -1;
                if(a.firstVerse > b.firstVerse) return 1;
                return 0;
            });
        }

        vm.sortByDateCreated = function() {
            vm.passages.sort(function(a,b) {
                if(a.dateCreated > b.dateCreated) return -1;
                if(a.dateCreated < b.dateCreated) return 1;
                return 0;
            });
        }
    }
})();