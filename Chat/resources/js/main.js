
/*
$(document).ready(function () { 
$("#post").click(function () {
    console.log("clickt");
    var output =  { VerfasserName: $('#Nameinput option:selected').val(),MessageText:$('#MessageText').val()};

    //$('#MessageText').val();
    //$('#Nameinput option:selected').val();
    $.post("api/employee", output, function (data) { $('.message-box').scrollTop($('.message-box')[0].scrollHeight);});
});
}); 

window.setInterval(function () {
  
    $.ajax({
        type: 'GET',
        url: 'api/employee',
        data: { get_param: 'value' },
        dataType: 'json',
        success: function (data) {
            var outputt;
            $.each(data, function (index, message) {
                console.log(message.verfasserName);
                if (message.verfasserName == "Lukas") {
                    var ausrichtung = "message-links";
                }
                if (message.verfasserName == "Dennis") {
                    var ausrichtung = "message-rechts";
                }
                outputt += '<div class="' + ausrichtung + '"><div class="message-header"><p style="padding-left:10px;">' + message.verfasserName + ' </p><p style="text-align: right; margin-top:-15px;padding-right:10px;"></p><br /></div><p style="padding:10px;">' + message.messageText + '</p></div><br>';

            });
            $('.message-box').html(
            
            
            
            
            t);
        }
    });
    $('.message-box').scrollTop($('.message-box')[0].scrollHeight);
}, 1000);



*/

  $(function () {
            "use strict";
            // for better performance - to avoid searching in DOM
            var content = $('#content');
            var input = $('#input');
            var status = $('#status');
            // my color assigned by the server
            var myColor = false;
            // my name sent to the server
            var myName = false;
            // if user is running mozilla then use it's built-in WebSocket
            window.WebSocket = window.WebSocket || window.MozWebSocket;
            // if browser doesn't support WebSocket, just show
            // some notification and exit
            if (!window.WebSocket) {
                content.html($('<p>',
                    { text: 'Sorry, but your browser doesn\'t support WebSocket.' }
                ));
                input.hide();
                $('span').hide();
                return;
            }
            // open connection
            var connection = new WebSocket('ws://127.0.0.1:1337');
            connection.onopen = function () {
                // first we want users to enter their names
                input.removeAttr('disabled');
                status.text('Wähle deinen Namen:');
            };
            connection.onerror = function (error) {
                // just in there were some problems with connection...
                content.html($('<p>', {
                    text: 'Sorry, but there\'s some problem with your '
                        + 'connection or the server is down.'
                }));
            };
            // most important part - incoming messages
            connection.onmessage = function (message) {
                // try to parse JSON message. Because we know that the server
                // always returns JSON this should work without any problem but
                // we should make sure that the massage is not chunked or
                // otherwise damaged.
                try {
                    var json = JSON.parse(message.data);
                } catch (e) {
                    console.log('Invalid JSON: ', message.data);
                    return;
                }
                // NOTE: if you're not sure about the JSON structure
                // check the server source code above
                // first response from the server with user's color
                if (json.type === 'color') {
                    myColor = json.data;
                    status.text(myName + ': ').css('color', myColor);
                    input.removeAttr('disabled').focus();
                    // from now user can start sending messages
                } else if (json.type === 'history') { // entire message history
                    // insert every single message to the chat window
                    for (var i = 0; i < json.data.length; i++) {
                        addMessage(json.data[i].author, json.data[i].text,
                            json.data[i].color, new Date(json.data[i].time));
                    }
                } else if (json.type === 'message') { // it's a single message
                    // let the user write another message
                    input.removeAttr('disabled');
                    addMessage(json.data.author, json.data.text,
                        json.data.color, new Date(json.data.time));
                } else {
                    console.log('Hmm..., I\'ve never seen JSON like this:', json);
                }
            };
            /**
             * Send message when user presses Enter key
             */
            input.keydown(function (e) {
                if (e.keyCode === 13) {
                    var msg = $(this).val();
                    if (!msg) {
                        return;
                    }
                    // send the message as an ordinary text
                    connection.send(msg);
                    $(this).val('');
                    // disable the input field to make the user wait until server
                    // sends back response
                    input.attr('disabled', 'disabled');
                    // we know that the first message sent from a user their name
                    if (myName === false) {
                        myName = msg;
                    }
                }

            });
            $(".inputbutton").click(function () {
                console.log("clickt");
                    var msg = $('.textbox').val();
                    if (!msg) {
                        return;
                    }
                    // send the message as an ordinary text
                    connection.send(msg);
                    $('.textbox').val('');
                    // disable the input field to make the user wait until server
                    // sends back response
                    input.attr('disabled', 'disabled');
                    // we know that the first message sent from a user their name
                    if (myName === false) {
                        myName = msg;
                    }
                       $('.message-box').scrollTop($('.message-box')[0].scrollHeight);
                });
            /**
             * This method is optional. If the server wasn't able to
             * respond to the in 3 seconds then show some error message
             * to notify the user that something is wrong.
             */
            setInterval(function () {
                if (connection.readyState !== 1) {
                    status.text('Error');
                    input.attr('disabled', 'disabled').val(
                        ' WebSocket Server nicht ereichbar.');
                }
            }, 3000);
            /**
             * Add message to the chat window
             */
            function addMessage(author, message, color, dt) {
                
                $('.message-box').append('<div class="message-links"><div class="message-header"><p style="padding-left:10px;">' + author + ' </p><p style="text-align: right; margin-top:-15px;padding-right:10px;">' + (dt.getHours() < 10 ? '0' + dt.getHours() : dt.getHours()) + ':'+ (dt.getMinutes() < 10? '0' + dt.getMinutes() : dt.getMinutes())+ '</p><br /></div><p style="padding:10px;">' + message + '</p></div><br>');
                //content.prepend('<p><span style="color:' + color + '">'
                //    + author + '</span> a ' + (dt.getHours() < 10 ? '0'
                //        + dt.getHours() : dt.getHours()) + ':'
                //    + (dt.getMinutes() < 10
                //        ? '0' + dt.getMinutes() : dt.getMinutes())
                //    + ': ' + message + '</p>');
            }
        });
