// reviewDetailController.js 

(function (){
    "use strict";
    
    // getting existing module
    angular.module("app-review").controller("reviewDetailController", reviewDetailController);

    function reviewDetailController($routeParams, $http, $location, $scope) {
        var vm = this;
        var url = "/api/review";
        vm.errorMessage = ""; 
        vm.isBusy = true;
        vm.id = $routeParams.id;
        
        vm.passage = {};
        vm.verses = {};
        vm.topics = [];
        vm.passedToday = true;
        vm.hidden = false; 

        $http.get("/api/passages/detail/"+vm.id) 
            .then(function (response) {
                angular.copy(response.data, vm.passage);
                vm.verses = JSON.parse(JSON.stringify(vm.passage.verses));
                vm.topics = JSON.parse(JSON.stringify(vm.passage.passageTopics));
                vm.topics.sort(function(a,b) {
                    if(a.title < b.title) return -1;
                    if(a.title > b.title) return 1;
                    return 0;
                });
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
                    //$location.path("#/");
                    history.back();
                }, function (error) {
                    vm.errorMessage = "Failed to pass passage: " + error; 
                });

        };

        vm.setLevel = function() {
            var level = document.getElementById("level").value;
            $http.post(url+"/setlevel/"+vm.id+"/"+level)
            .then(function (response){
                //
            }, function (error) {
                vm.errorMessage = "Failed to set level: " + error; 
            });
        }

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