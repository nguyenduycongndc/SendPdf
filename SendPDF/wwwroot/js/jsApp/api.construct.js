$(function () {
    $.ajaxSetup({
        beforeSend: function (xhr) {
            var rd = Math.floor((Math.random() * 9999999) + 1);
            xhr.setRequestHeader('Authorization', getSessionToken());
            xhr.setRequestHeader('Accept-Language', 'vi-VN');
            xhr.setRequestHeader('Loading-Id', rd);
            showLoading();
        },
        complete: function (xhr, status, error) {
            if (xhr.status == 401)
                //swal("Unauthorized!", "Bạn cần phải đăng nhập vào hệ thống!", "warning");
                toastr.error("Dữ liệu đầu vào hoặc thông tin tài khoản không hợp lệ!", "Lỗi!", { progressBar: true });
            else if (xhr.status == 404)
                toastr.error("Không tìm thấy đối tượng để xử lý!", "Lỗi!", { progressBar: true });
            else if (xhr.status == 500)
                toastr.error("Có lỗi xảy ra trong quá trình xử lý!", "Lỗi!", { progressBar: true });
            else if (xhr.status == 400)
                toastr.error("Dữ liệu đầu vào hoặc thông tin tài khoản không hợp lệ!", "Lỗi!", { progressBar: true });
        }
    });

    $(document).ajaxStop(function () {
        hideLoading();
    });
})
function getSessionToken() {
    if (sessionStorage['SessionToken'] != undefined)
        return 'Bearer ' + sessionStorage['SessionToken'];
    return null;
}

function callApi(controller, action, method, data, callbackSuccess, callbackError) {
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
}

function callUpload(controller, action, formData, callbackSuccess, callbackError) {
    $.ajax({
        //"url": apiConfig.api.host + controller + action,
        "url": apiConfig.api.host_user_service + controller + action,
        "contentType": false,
        "processData": false,
        "dataType": 'json',
        "type": "POST",
        "cache": false,
        "data": formData,
        success: function (result) {
            if (window[callbackSuccess] != undefined)
                window[callbackSuccess](result);
        },
        error: function (request, status, error) {
            if (window[callbackError] != undefined)
                window[callbackError](request, status, error);
        }
    });
}
