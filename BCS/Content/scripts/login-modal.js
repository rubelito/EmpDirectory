$(document).ready(function () {
    var ShowError = function (model) {
        $('#error-message').html("");
        model.ErrorMessages.forEach(function(element){
            $('#error-message').append("<span>" + element + "</span><br />");
        });
    };


    $('#login-modal').on('show.bs.modal', function () {
        $('#login-button').on('click', function () {
            var userName = $('#user-name-text').val();
            var password = $('#password-text').val();
            var rememberMe = $('#remember-me-check').val();

            var model =
            {
                UserName: userName,
                Password: password,
                RememberMe: rememberMe
            };

            $.ajax({
                type: "Post",
                url: "/Home/Login/",
                data: JSON.stringify(model),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (status) {
                    if (status.IsSuccessful) {
                        window.location.href = "/Employees/";
                    }
                    else {
                        ShowError(status);
                    }
                },
            });
        });
    });
});