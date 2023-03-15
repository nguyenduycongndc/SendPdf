function callApi_userservice(controller, action, method, data, callbackSuccess, callbackError) {
    $.ajax({
        type: method,
        url: apiConfig.api.host_user_service + controller + action,
        contentType: "application/json; charset=utf-8",
        data: (method == 'GET' ? data : JSON.stringify(data)),
        success: function (result) {
            if (window[callbackSuccess] != undefined)
                window[callbackSuccess](result);
        },
        error: function (request, status, error) {
            if (window[callbackError] != undefined)
                window[callbackError](request, status, error);
        }
    });
};
