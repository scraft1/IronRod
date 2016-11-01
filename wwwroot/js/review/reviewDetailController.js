// reviewDetailController.js 

(function (){
    "use strict";
    
    // getting existing module
    angular.module("app-review").controller("reviewDetailController", reviewDetailController);

    function reviewDetailController($routeParams, $http, $location) {
        var vm = this;
        var url = "/api/passages";
        vm.errorMessage = ""; 
        vm.isBusy = true;
        vm.id = $routeParams.id;
        
        vm.passage = {};
        vm.verses = {};
        vm.passedToday = true;
        vm.hidden = false; 

        $http.get(`${url}/detail/${vm.id}`) 
            .then(function (response) {
                angular.copy(response.data, vm.passage);
                vm.verses = JSON.parse(JSON.stringify(vm.passage.verses));

                var dp = Date.parse(vm.passage.datePassed);
                var today = Date.parse(new Date().setHours(0,0,0,0));
                vm.passedToday = dp == today && vm.passage.level != 0;
            }, function (error) { 
                vm.errorMessage = "Failed to load data: " + error;
            })
            .finally(function(){
                vm.isBusy = false;
            });

        vm.passed = function(){
            $http.post(url+"/passed/"+vm.passage.id) // optional second parameter for post contents
                .then(function (response){
                    $location.path("#/");
                }, function (error) {
                    vm.errorMessage = "Failed to pass passage: " + error; 
                });

        };

        vm.hideWords = function(){
            if(vm.hidden){
                vm.verses = JSON.parse(JSON.stringify(vm.passage.verses));
                vm.hidden = false;
            } else { 
                for(var v = 0; v < vm.verses.length; v++){
                    var words = vm.verses[v].verseText.split(" ");
                    vm.verses[v].verseText = "";
                    for(var w = 0; w < words.length; w++){
                        var letters = words[w].split("");
                        for(var l = 1; l < letters.length; l++){
                            if(!/[\-,.;:()'"?!]/.test(letters[l])){
                                letters[l] = '_';
                            }
                        }
                        var word = letters.join(""); 
                        vm.verses[v].verseText += word+" ";
                    }
                    vm.verses[v].verseText.slice(0,-1);
                }
                vm.hidden = true;
            }
        }
    }
})();