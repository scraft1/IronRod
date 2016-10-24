// simpleControls.js 

(function (){
    "use strict";

    // creating the module 
    angular.module("simpleControls", [])
        .directive("waitCursor", waitCursor);

    function waitCursor() {
        return {
            scope: {
                show: "=displayWhen"
            },
            restrict: "E", // must be its own element, not a property 
            // templateUrl: "/views/waitCursor.html"
            template: `<div ng-show="show" class="text-center">
                            <i class="fa fa-spinner fa-spin"></i>
                            Loading...
                        </div>`
        };
    }
})();