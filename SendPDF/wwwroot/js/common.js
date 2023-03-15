function msgError(respon, status, error) {
    if (respon.status == 401)
        toastr.warning("Bạn phải đăng nhập vào hệ thống trước khi thao tác", "Lỗi", { progressBar: true })
    else if (respon.status == 404)
        toastr.warning("Không tìm thấy đối tượng để xử lý!", "Lỗi!", { progressBar: true });
    else
        toastr.error("Có lỗi xảy ra trong quá trình xử lý!", "Lỗi", { progressBar: true })
}

function formatNumberByLocate(value) {
    if (value == undefined || value == null || value == "")
        return "0";
    return Number(value).toLocaleString('vi')
}

function showLoading() {
    $('#preloader').css('display', 'block');
}

function hideLoading() {
    setTimeout(function () {
        var keys = Object.keys(localStorage);
        var isOk = true;
        for (var i = 0; i < keys.length; i++) {
            if (keys[i].startsWith('loading'))
                isOk = false;
        }
        if (isOk)
            $('#preloader').css('display', 'none');
    }, 500);
}

function clearMsgInvalid() {
    var inputInvalid = $('.is-invalid');
    var labelInvalid = $('.invalid-feedback');
    for (var i = 0; i < inputInvalid.length; i++) {
        $(inputInvalid[i]).removeClass('is-invalid');
    }
    for (var i = 0; i < labelInvalid.length; i++) {
        $(labelInvalid[i]).remove();
    }
}

function validateRequired(parent) {
    parent = !parent ? '' : parent;
    var allRequired = $(parent + ' .required');
    var isValid = true;
    var itemFocus = null;
    for (var i = 0; i < allRequired.length; i++) {
        var grInput = $(allRequired[i]).parent().find('input');
        var grSelect = $(allRequired[i]).parent().find('select');
        var grTextarea = $(allRequired[i]).parent().find('textarea');

        var msg = $(allRequired[i]).parent().find('label').text() + ' ' + "Không được để trống";
        $(allRequired[i]).parent().find('.invalid-feedback').remove();

        if (grInput.length > 0 || grSelect.length > 0 || grTextarea.length > 0) {
            var eleWork = grInput.length > 0 ? grInput : grSelect.length > 0 ? grSelect : grTextarea;
            eleWork.removeClass('is-invalid');

            var val = eleWork.val();

            if (val == undefined || val == null || val == ""/* || val.isBlank()*/) {
                itemFocus = itemFocus == null ? $(allRequired[i]) : itemFocus;
                eleWork.addClass('is-invalid');
                $(eleWork).parent().append('<div class="invalid-feedback">' + msg + '</div>')
                isValid = false;
            }
        }
    }
    if (!isValid)
        itemFocus.focus();
    return isValid;
}

function fnChangeHeader(action) {
    if (action == 'view')
        $('#header-update').text($('#header-update').text().replace(localizationResources.Update, localizationResources.Detail))
    else
        $('#header-update').text($('#header-update').text().replace(localizationResources.Detail, localizationResources.Update))
}

function callApi_multipleselect(selector, placeholder, host, controller, action) {
    $("#" + selector).select2({
        placeholder: placeholder,
        minimumInputLength: 0,
        multiple: true,
        closeOnSelect: false,
        ajax: {
            headers: { "Authorization": "Bearer " + sessionStorage['SessionToken'] },
            url: host + controller + action,
            dataType: 'json',
            data: function (params) {
                var query = {
                    q: params.term,
                    type: 'public'
                }
                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data.data, function (item) {
                        return {
                            text: item.full_name ?? item.name,
                            id: item.id
                        }
                    })
                };
            },
            cache: true
        }
    });
}
function callApi_select(selector, placeholder, host, controller, action) {
    $("#" + selector).select2({
        placeholder: placeholder,
        minimumInputLength: 0,
        multiple: false,
        closeOnSelect: true,
        ajax: {
            headers: { "Authorization": "Bearer " + sessionStorage['SessionToken'] },
            url: host + controller + action,
            dataType: 'json',
            data: function (params) {
                var query = {
                    q: params.term,
                    type: 'public'
                }
                return query;
                return {
                    text: params.term
                };
            },
            processResults: function (data) {
                return {
                    results: $.map(data.data, function (item) {
                        return {
                            text: item.full_name ?? item.name,
                            id: item.id
                        }
                    })
                };
            },
            cache: true
        }
    });
}
function ResetPageSize() {
    $("#txtCurrentPage").val(1);
    reCalculatPagesCustom(0);
    viewBtnActionPage();
}

function callApi_oneselect(selector, placeholder, host, controller, action) {
    $("#" + selector).select2({
        placeholder: placeholder,
        minimumInputLength: 0,
        multiple: false,
        closeOnSelect: true,
        ajax: {
            headers: { "Authorization": sessionStorage['SessionToken'] },
            url: host + controller + action,
            dataType: 'json',
            data: function (params) {
                var query = {
                    q: params.term,
                    type: 'public'
                }
                return query;
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            id: item.id,
                            text: item.address,
                        }
                    })
                };
            },
            cache: true
        }
    });
}

