// app-passage.js 

(function (){
    "use strict";

    // creating the module 
    angular.module("app-passages", ["simpleControls", "ngRoute"])
        .config(function ($routeProvider) {

            $routeProvider.when("/", {
                controller: "passagesController", 
                controllerAs: "vm",
                templateUrl: "/views/passagesView.html"
            });
            // $routeProvider.when("/:title", {
            //     controller: "passageDetailController", 
            //     controllerAs: "vm",
            //     templateUrl: "/views/passageDetailView.html"
            // });

            $routeProvider.otherwise({ redirectTo: "/"});

        });
})();