/**
 * Created by rubelitoisiderio on 9/7/18.
 */
var module = angular.module("EmployeeApp");

module.controller('EmployeeController', ['$scope', '$log', 'EmployeeService', '$location', '$routeParams', '$uibModal', function ($scope, $log, EmployeeService, $location, $routeParams, $uibModal) {
    var $ctrl = this;
    $ctrl.animationsEnabled = true;

    $scope.search = '';
    $scope.Result = null;
    $scope.OperationMessage = '';
    $scope.ShowStateDropdown = false;
    $scope.ShowStateTextbox = false;
    $scope.tempState = '';
    $scope.Employee = null;

    $scope.CurrentPage = 1;
    $scope.MaxSize = 10;
    $scope.ItemsPerPage = 6;
    $scope.Ordeyby = 'Descending';
    $scope.OrderbyColumn = 'Id';

    $scope.PageChanged = function () {
        var promise = EmployeeService.Search($scope.CurrentPage, $scope.ItemsPerPage, $scope.Ordeyby, $scope.OrderbyColumn, $scope.SearchStr);

        promise.then(function (data) {
            $scope.IsAdmin = data.IsAdmin;
            $scope.ShouldDisplayAddAndEdit = data.ShouldDisplayAddAndEdit;
            $scope.TotalItems = data.TotalRecords;
            $scope.Users = data.Models;
        })
    }

    $scope.GetAllUser = function () {
        $scope.PageChanged();
    }

    $scope.OrdeybyChanged = function () {
        if ($scope.Ordeyby == 'Descending')
            $scope.Ordeyby = 'Ascending'
        else
            $scope.Ordeyby = 'Descending';

        $scope.GetAllUser();
    }

    $scope.OrderbyCriteria = function(column){
        $scope.OrderbyColumn = column;
        $scope.OrdeybyChanged();
    }

    $scope.Search = function(){
        $scope.Ordeyby = 'Ascending';
        $scope.OrderbyColumn = 'Alphabetical';
        $scope.GetAllUser();
    }

    $scope.AddNew = function () {
        $scope.EmployeeModel = new Object();
        $scope.ShowStateDropdown = false;
        $scope.ShowStateTextbox = false;
        $scope.addEmployeeModel = {
            UserType: 0,
            UserTypeList: [{value: 0, name: 'Employee'},
                {value: 1, name: 'Viewer'},
                {value: 2, name: 'Editor'},
                {value: 3, name: 'Admin'}],
            Gender: 0,
            GenderList: [{value: 0, name: 'Male'},
                {value: 1, name: 'Female'}],
            CivilStatus: 1,
            CivilStatusList: [{value: 0, name: 'None'},
                {value: 1, name: 'Single'},
                {value: 2, name: 'Married'}],
            Country: 'United States of America',
            Countries: []
        };
        $scope.EmployeeModel = $scope.addEmployeeModel;
        $scope.EmployeeModel.Operation = 'add';
        $scope.ShowEditForm();
        GetCountries($scope.EmployeeModel, true);
    }

    $scope.beginEdit = function (id) {
        $location.path("EditUser/" + id);
        $scope.ShowStateDropdown = false;
        $scope.ShowStateTextbox = false;
        $scope.EmployeeModel = new Object();
        var emp = EmployeeService.GetUserById(id);
        emp.then(function (data) {
            $scope.EmployeeModel = data;
            if (data.BirthDate) {
                $scope.EmployeeModel.BirthDate = new Date(parseInt(data.BirthDate.substr(6)));
            }
            $scope.EmployeeModel.Operation = 'edit';
            $scope.ShowEditForm();

            GetCountries($scope.EmployeeModel, false);
        });
    }

    $scope.ShowUserDetail = function(id){
        $location.path("UserDetail/" + id);
        $scope.EmployeeModel = new Object();
        var emp = EmployeeService.GetUserDetail(id);
        emp.then(function(data){
            $scope.EmployeeModel = data;
            $scope.ShowDetailForm();

            GetCountries($scope.EmployeeModel, false);
        });
    }

    function GetCountries(employee, shouldClear) {
        var countryPromise = EmployeeService.GetCountries();
        countryPromise.then(function (data) {
            employee.Countries = data;
            GetState(employee, shouldClear);
        })
    }

    function GetState(employee, shouldClear) {
        if (employee.Country == 'Philippines' || employee.Country == 'United States of America') {
            $scope.ShowStateTextbox = false;
            $scope.ShowStateDropdown = true;

            var statePromise = EmployeeService.GetStates(employee.Country);
            statePromise.then(function (data) {
                employee.States = data;

                if (shouldClear) {
                    employee.State = data[0].Name;
                }
            })
        }
        else {
            if (shouldClear) {
                employee.State = '';
            }
            $scope.ShowStateDropdown = false;
            $scope.ShowStateTextbox = true;
        }
        $scope.tempState = employee.State;
    }

    $scope.CountryChange = function () {
        GetState($scope.EmployeeModel, true);
    }

    $scope.AddUser = function (employee) {
        var emp = EmployeeService.AddUser(employee);
        emp.then(function (data) {
            if (data.Success == true) {
                $scope.GetAllUser();
            }
            $scope.Result = data;
        })
    }

    $scope.EditUser = function (employee) {
        var emp = EmployeeService.EditUser(employee);
        emp.then(function (data) {
            if (data.Success == true) {
                $scope.GetAllUser();
            }
            $scope.Result = data;
        })
    }

    $scope.ShowEditForm = function () {
        $scope.message = "Show Form Button Clicked";
        console.log($scope.message);

        var modalInstance = $uibModal.open({
            templateUrl: '/Content/View/EditUser.html',
            controller: employeeModalControlller,
            scope: $scope,
            resolve: {
                employeeModel: function () {
                    return $scope.EmployeeModel;
                }
            }
        });

        modalInstance.result.then(function (employee) {
            if (employee.Operation == 'add') {
                $scope.AddUser(employee);
            }
            else if (employee.Operation == 'edit') {
                $scope.EditUser(employee);
            }
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
            $scope.tempState = '';
        });
    };

     $scope.ShowDetailForm = function () {
        $scope.message = "Show Form Button Clicked";
        console.log($scope.message);

        var modalInstance = $uibModal.open({
            templateUrl: '/Content/View/UserDetail.html',
            controller: employeeModalControlller,
            scope: $scope,
            resolve: {
                employeeModel: function () {
                    return $scope.EmployeeModel;
                }
            }
        });

        modalInstance.result.then(function (employee) {
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
            $scope.tempState = '';
        });
    };

    $scope.BirthdateIsOpen = {
        opened: false
    };

    $scope.BirthDatePickupOpen = function () {
        $scope.BirthdateIsOpen.opened = true;
    };

    $scope.ChangedActive = function (user) {
        if (user.IsActive) {
            EmployeeService.EnableUser(user.Id);
        }
        else {
            EmployeeService.DisableUser(user.Id);
        }
    }

    $scope.UnlockUser = function (user) {
        var promise = EmployeeService.UnlockUser(user.Id);
        promise.then(function (result) {
            if (result == 'True') {
                user.IsLock = false;
            }
        });
    }

    $scope.GetAllUser();
}]);