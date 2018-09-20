/**
 * Created by rubelitoisiderio on 9/8/18.
 */
var employeeModalControlller = function ($scope, $uibModalInstance, employeeModel) {
    $scope.submitForm = function () {
            $uibModalInstance.close(employeeModel);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
};