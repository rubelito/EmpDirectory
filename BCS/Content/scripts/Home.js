
var HomeApp = angular.module('HomeApp', []);
HomeApp.controller('HomeController', ['$scope', '$http', function($scope, $http){

    var paramData = JSON.stringify({product: 'rubelito', desc: '43'});

    var config = {
        headers: {
            'Content-Type' : 'application/json'
        }
    };

    var postPromise = $http.post(
        '/Home/GetPerson',
        paramData, config);
    postPromise.then(function(response){
       $scope.driver = response.data;
    });
    postPromise.catch(function(error){
       $scope.error = error;
    });
}]);