/**
 * Created by rubelitoisiderio on 9/7/18.
 */
var app = angular.module('EmployeeApp', ['ngRoute', 'ngAnimate', 'ngSanitize','ui.bootstrap', 'uiToggle']);

app.config(function ($routeProvider) {
    $routeProvider
        .when("/EditUser/:id", {
            templateUrl: '/Content/View/EditUser.html',
            controller: "EmployeeController"
            })
        .when("/UserDetail/:id", {
            templateUrl: '/Content/View/UserDetail.html',
            controller: "EmployeeController"
            })
        .otherwise({redirectTo: "/Employees"});
});