// app-review.js 

(function (){
    "use strict";

    // creating the module 
    angular.module("app-review", ["simpleControls", "ngRoute"])
        .config(function ($routeProvider) {

            $routeProvider.when("/", {
                controller: "reviewTopicsListController", 
                controllerAs: "vm",
                templateUrl: "/views/reviewTopicsListView.html"
            });
            $routeProvider.when("/Topic/:id", {
                controller: "reviewTopicPassagesController", 
                controllerAs: "vm",
                templateUrl: "/views/reviewTopicPassagesView.html"
            });
            $routeProvider.when("/Passage/:id", {
                controller: "reviewDetailController", 
                controllerAs: "vm",
                templateUrl: "/views/reviewDetailView.html"
            });

            $routeProvider.otherwise({ redirectTo: "/"});

        });
})();