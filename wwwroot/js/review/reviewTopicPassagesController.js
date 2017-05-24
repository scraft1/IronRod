// reviewListController.js 

(function (){
    "use strict";
    
    // getting existing module
    angular.module("app-review").controller("reviewTopicPassagesController", reviewTopicPassagesController);

    function reviewTopicPassagesController($http, $routeParams) {
        var vm = this;
        vm.passages = [];
        vm.title = $routeParams.title;
        vm.errorMessage = ""; 
        vm.isBusy = true;
        vm.id = $routeParams.id;

        $http.get(`/api/review/topic/${vm.id}`) 
            .then(function (response) { 
                angular.copy(response.data, vm.passages);
                vm.passages.sort(function(a,b) {
                    if(a.firstVerse < b.firstVerse) return -1;
                    if(a.firstVerse > b.firstVerse) return 1;
                    return 0;
                });
            }, function (error) { 
                vm.errorMessage = "Failed to load data: " + error;
            })
            .finally(function(){
                vm.isBusy = false;
            });
        
    }

})();