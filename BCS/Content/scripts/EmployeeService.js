/**
 * Created by rubelitoisiderio on 9/7/18.
 */
var module = angular.module('EmployeeApp');

module.factory('EmployeeService', ['$http', function ($http) {
    var config = {
        headers: {
            'Content-Type': 'application/json'
        }
    };

    var config1 = {
      headers: {
          'Content-Type': 'application/x-www-form-urlencoded'
      }
    };

    var getUserById = function (id) {
        var promise = $http.get("http://127.0.0.1:8080/Employees/GetUserById/" + id);

        return promise.then(function (response) {
            response.data.UserTypeList = [{value: 0, name: 'Employee'},
                {value: 1, name: 'Viewer'},
                {value: 2, name: 'Editor'},
                {value: 3, name: 'Admin'}]

            response.data.GenderList = [
                {value: 0, name: 'Male'},
                {value: 1, name: 'Female'}]

            response.data.CivilStatusList = [
                {value: 0, name: 'None'},
                {value: 1, name: 'Single'},
                {value: 2, name: 'Married'}]

            return response.data;
        });
    };

    var addUser = function (userToAdd) {
        var token = angular.element('input[name="__RequestVerificationToken"]').attr('value');
        $http.defaults.headers.common['__RequestVerificationToken'] = token;

        var postPromise = $http.post(
            'http://127.0.0.1:8080/Employees/AddPerson',
            userToAdd, config);
        return postPromise.then(function (response) {
            return response.data;
        });
    }

    var getAllUser = function () {
        var promise = $http.get('http://127.0.0.1:8080/Employees/GetAllUser/');

        return promise.then(function (response) {
            return response.data;
        })
    }

    var getUserDetail = function(id) {
        var promise = $http.get('http://127.0.0.1:8080/Employees/Details/' + id);

        return promise.then(function (response) {
            return response.data;
        });
    };

    var editUser = function (userToEdit) {
        var token = angular.element('input[name="__RequestVerificationToken"]').attr('value');
        $http.defaults.headers.common['__RequestVerificationToken'] = token;
    
        var postPromise = $http.post('http://127.0.0.1:8080/Employees/EditUser', userToEdit, config);
        return postPromise.then(function (response) {
            return response.data;
        })
    }

    var getCountries = function(){
        var promise = $http.get('http://127.0.0.1:8080/Employees/GetListOfCountry');

        return promise.then(function (response){
            return response.data;
        });
    }

    var getStates = function(country){
        var param = angular.toJson({country: country});
        var postPromise = $http.post('http://127.0.0.1:8080/Employees/GetStates', param, config);

        return postPromise.then(function(response){
            return response.data;
        });
    }

    var enableUser = function(id){
        var token = angular.element('input[name="__RequestVerificationToken"]').attr('value');
        $http.defaults.headers.common['__RequestVerificationToken'] = token;

        var promise = $http.get('http://127.0.0.1:8080/Employees/Enable/' + id);

        return promise.then(function (response) {
            return response.data;
        });
    }

    var disableUser = function(id){
        var token = angular.element('input[name="__RequestVerificationToken"]').attr('value');
        $http.defaults.headers.common['__RequestVerificationToken'] = token;

        var promise = $http.get('http://127.0.0.1:8080/Employees/Disable/' + id);

        return promise.then(function(response){
            return response.data;
        });
    }

    var unlockUser = function(id){
        var token = angular.element('input[name="__RequestVerificationToken"]').attr('value');
        $http.defaults.headers.common['__RequestVerificationToken'] = token;

        var promise = $http.get('http://127.0.0.1:8080/Employees/Unlock/' + id);

        return promise.then(function(response){
            return response.data;
        });
    }

    var search = function(currentPage, pageSize, orderBy, orderByColumn, searchStr)
    {
        var promise = $http.get('http://127.0.0.1:8080/Employees/Search/', {params: {page: currentPage, pageSize: pageSize, orderBy: orderBy, orderByColumn: orderByColumn, search: searchStr}});

        return promise.then(function (response) {
            return response.data;
        })
    }

    return {
        GetUserById: getUserById,
        AddUser: addUser,
        EditUser: editUser,
        GetAllUser: getAllUser,
        GetCountries: getCountries,
        GetStates: getStates,
        EnableUser: enableUser,
        DisableUser: disableUser,
        UnlockUser: unlockUser,
        Search: search,
        GetUserDetail: getUserDetail
    }
}]);