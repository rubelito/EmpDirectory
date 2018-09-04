/**
 * Created by rubelitoisiderio on 8/17/18.
 */

$(document).ready(function () {
    window.setInterval(function () {
        var dt = new Date();
        var seconds = dt.getSeconds() > 9 ? dt.getSeconds() : '0' + dt.getSeconds();
        var minutes = dt.getMinutes() > 9 ? dt.getMinutes() : '0' + dt.getMinutes();
        var hour = dt.getHours();

        $('.news-time').html(hour + ':' + minutes + ':' + seconds);

    }, 1000);

    $('#Unlock-User-Modal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget)
        var id = button.closest("tr")
            .find(".table-row-id")
            .html().trim();

        $('#idToUnlock').val(id);

        var username = button.closest("tr")
            .find(".table-row-username")
            .html().trim();

        $('#Unload-User-Message').html(username);
    });

    $('#Unlock-User-Button').on('click', function () {
        var idToUnlock = $('#idToUnlock').val();
        var antiForgeryToken = $('[name=__RequestVerificationToken]').val();
        var dataToSend = {};

        dataToSend["id"] = idToUnlock;
        dataToSend["__RequestVerificationToken"] = antiForgeryToken;

        var url = url = "/Employees/Unlock";

        $.post(url, dataToSend)
            .done(function (succeed) {
                if (succeed == 'True') {
                    window.location.href = "/Employees/";
                }
                else if (succeed == 'False') {
                    alert("Error Unlocking User !");
                }
            });
    });

    $('#Enable-Disable-employee-modal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget)
        var operation = button.text();
        $('#hiddenOperation').val(operation);


        var id = button.closest("tr")
            .find(".table-row-id")
            .html().trim();

        $('#idToActivateOrDeactivate').val(id);

        var username = button.closest("tr")
            .find(".table-row-username")
            .html().trim();

        if (operation == "Enable")
        {
            $('#exampleModalLabel').html("Active Employee");
            $('#operation-message').html("Do you want to activate");
            $('#Enable-Disable-employee-button').html("Activate!");
        }
        else if (operation == "Disable")
        {
            $('#exampleModalLabel').html("Deactivate Employee");
            $('#operation-message').html("Do you want to deactivate");
            $('#Enable-Disable-employee-button').html("Deactivate!");
        }

        $('#delete-employee-message').html(username);
    });

    $('#Enable-Disable-employee-button').on('click', function () {
        var idToActivate = $('#idToActivateOrDeactivate').val();
        var operation = $('#hiddenOperation').val();
        var antiForgeryToken = $('[name=__RequestVerificationToken]').val();
        var dataToSend = {};

        dataToSend["id"] = idToActivate;
        dataToSend["__RequestVerificationToken"] = antiForgeryToken;


        var url = "";
        if (operation == "Enable")
        {
            url = "/Employees/Enable";
        }
        else if (operation == "Disable")
        {
            url = "/Employees/Disable";
        }

        $.post(url, dataToSend)
            .done(function (succeed) {
                if (succeed == 'True') {
                    window.location.href = "/Employees/";
                }
                else if (succeed == 'False') {
                    alert("Error Enabling/Disable !");
                }
            });
    });

    $('.datepicker').datepicker(); //Initialise any date picker

    $('.editor-label').has('.field-validation-error').find('.form-control').css('border', '1px solid #DC3545');

    setColumnArrow();
});

function setColumnArrow() {
    var currentOrder = $('#idCurrentOrderBy').val();
    var currentColumn = $('#idCurrentColumn').val();

    var cssClassArrow = "";
    if (currentOrder == "Ascending") {
        cssClassArrow = "glyphicon glyphicon-triangle-top";
    }
    else {
        cssClassArrow = "glyphicon glyphicon-triangle-bottom";
    }

    if (currentColumn == "Id")
    {
        $('#id-Arrow').addClass(cssClassArrow);
    }
    else if (currentColumn == "HireDate"){
        $('#hireDate-Arrow').addClass(cssClassArrow);
    }
    else if (currentColumn == "Department"){
        $('#department-Arrow').addClass(cssClassArrow);
    }
    else if (currentColumn == "Alphabetical"){
        $('#alphabetical-Arrow').addClass(cssClassArrow);
    }
}
