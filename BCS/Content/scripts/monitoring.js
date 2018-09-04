$(function () {
    var chat = $.connection.monitorHub;
    $.connection.hub.logging = true;
    chat.client.activityLog = function (operation, message) {

        $('#discussion').append('<li><strong>' + htmlEncode(operation)
            + '</strong>: ' + htmlEncode(message) + '</li>');
    };

    $('#message').focus();

    $.connection.hub.start().done(function () {
        $('#sendmessage').click(function () {
            chat.server.send($('#message').val());
            $('#message').val('').focus();
        });
    });
});

function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}
