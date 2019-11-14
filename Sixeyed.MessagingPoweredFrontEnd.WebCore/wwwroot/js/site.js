// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
        
    var flipStyle = false;

    function bindConnectionMessage(connection, hubMethodName, hubMethodCallback) {        
        // Create a function that the hub can call to broadcast messages.
        connection.on(hubMethodName, hubMethodCallback);
        connection.onclose(onConnectionError);
    }

    function onConnected(connection) {
        var userId = $('#Sender').val();
        connection.send('registerUser', userId);

        $('#btnSend').click(function () {
            connection.send('send', $('#Sender').val(), $('#Content').val());
            $('#Content').val('').focus();
        });

        connection.invoke('getMail').then(function (mailBag) {
            var mailBox = $('#mailBox');
            mailBox.empty();
            $.each(mailBag, function () {
                mailBox.append(format(this));
            });
        });
    }

    function onConnectionError(error) {
        if (error && error.message) {
            console.error(error.message);
        }
        //var modal = document.getElementById('myModal');
        //modal.classList.add('in');
        //modal.style = 'display: block;';
    }

    function format(mail) {
        var style = flipStyle ? 'warning' : 'success';
        flipStyle = !flipStyle;
        return '<div class="panel panel-' + style + '"><div class="panel-body">' + mail.Content + '</div><div class="panel-footer">From: <span class="text-' + style + '"><b>@' + mail.Sender + '</b></span>, at: ' + mail.SentAt + '</div></div>';
    }

    function newMail(mail) {
        var mailBox = $('#mailBox');
        mailBox.prepend(format(mail));
    }

    function showResponse(response) {
        var type = response.Event === 'broadcast' ? 'success' : 'info';
        //toastr[type]('Your mail from ' + response.SentAt + ' was ' + response.Event +
        //    ' at ' + response.EventAt);
    }

    
    var connection = new signalR.HubConnectionBuilder()
        .withUrl('/noticeboard')
        .build();

    bindConnectionMessage(connection, 'newMail', newMail);
    bindConnectionMessage(connection, 'showResponse', showResponse);

    connection.start()
        .then(function () {
            onConnected(connection);
        })
        .catch(function (error) {
            console.error(error.message);
        });

});

function send() {
    var noticeboard = $.connection.noticeboard;
    noticeboard.server.send($('#Sender').val(), $('#Content').val());
    $('#Content').val('').focus();
}