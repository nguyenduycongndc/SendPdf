function openView(type, value) {
    var index = $("#view");
    var create = $("#create");
    var edit = $("#edit");
    var detail = $("#detail");
    if (type === 0) {
        index.show();
        create.hide();
        edit.hide();
        setTimeout(function () {
            onSearch();
        }, 100);
    }
    else if (type === 1) {
        clearMsgInvalid();
        $('#emailAddressCreate').val(""),
            $('#CCCreate').val(""),
            localStorage.setItem("type", "1");
        index.hide();
        //detail.hide();
        create.show();
        //document.getElementById("userNameCreate").focus();
        edit.hide();
        //document.getElementById("formCreate").reset();
        //$("#frmHeaderCreate").val(frmHeaderCreate);
    }
    else if (type === 2 || type === 3) {
        index.hide();
        create.hide();
        edit.show();
        //detail.show();

        fnGetDetail(type, value);
    }
    //else if (type === 3) {
    //    clearMsgInvalid();
    //    index.hide();
    //    create.hide();
    //    //detail.hide();
    //    edit.show();
    //    //document.getElementById("userNameEdit").focus();
    //    fnGetDetail(type, value);
    //}
}

$(document).on('click', '#select_all', function () {
    $('.checkitem').not(this).prop('checked', this.checked);
    var _objList = $('input[class="checkitem"]:checked').map((_, el) => el.value).get();
    if (_objList.length > 0) {
        return $("#sendMailBtn").attr("disabled", false);
    } else {
        return $("#sendMailBtn").attr("disabled", true);
    }
});

function fnSearchDataSuccess(rspn) {
    showLoading();
    if (rspn !== undefined && rspn !== null && rspn.data.length > 0) {
        var tbBody = $('#emailTypeTable tbody');
        $("#emailTypeTable").dataTable().fnDestroy();
        tbBody.html('');
        for (var i = 0; i < rspn.data.length; i++) {
            var obj = rspn.data[i];
            var CC = obj.cc != null ? obj.cc : "";
            var CA = obj.create_at != null ? obj.create_at : "";
            var html = '<tr>' +
                '<td class="text-center"></td>' +
                '<td class=" dt-body-center">' +
                '<div class="dt-checkbox">' +
                '<input type="checkbox" class="checkitem" id="checkitem_' + i + '" value="' + obj.email_address + ';' + CC + '"><span class="dt-checkbox-label"></span>' +
                //(CC != "" ? 
                //    '<input type="checkbox" class="checkitem" id="checkitem_' + i + '" value="' + obj.email_address + ';' + CC + '"><span class="dt-checkbox-label"></span>' :
                //    '<input type="checkbox" class="checkitem" id="checkitem_' + i + '" value="' + obj.email_address + '"><span class="dt-checkbox-label"></span>'
                //) +
                '</div>' +
                '</td > ' +
                '<td>' + obj.email_address + '</td>' +
                '<td>' + CC + '</td>' +
                '<td>' + CA + '</td>' +
                '<td class="text-center">' +
                '<a type="button" class="btn icon-default btn-action-custom" onclick="openView(2,' + obj.id + ')" style="color:green"><i data-toggle="tooltip" title="Chi tiết" class="fa fa-eye" aria-hidden="true"></i></a>' +
                '<a type="button" class="btn icon-default btn-action-custom" onclick="openView(3,' + obj.id + ')" style="color:blue"><i data-toggle="tooltip" title="Cập nhật" class="micon dw dw-edit2" aria-hidden="true"></i></a>' +
                '<a type="button" class="btn icon-delete btn-action-custom" onclick="Delete(' + obj.id + ')" style="color:red"><i data-toggle="tooltip" title="Xóa" class="fa fa-trash" aria-hidden="true"></i></a>' +
                '</td>' +
                '</tr>';
            tbBody.append(html);

            $("#checkitem_" + i + "").click(function () {
                var _objList = $('input[class="checkitem"]:checked').map((_, el) => el.value).get();
                if (_objList.length > 0) {
                    return $("#sendMailBtn").attr("disabled", false);
                }
                if (_objList.length == rspn.data.length) {
                    $('#select_all').prop('checked', this.checked);
                } else {
                    $('#select_all').prop('checked', false);
                    return $("#sendMailBtn").attr("disabled", true);
                }
            });
        }

        var page_size = (parseInt($("#txtCurrentPage").val()) - 1) * parseInt($("#cbPageSize").val())
        var t = $("#emailTypeTable").DataTable({
            "bPaginate": false,
            "bLengthChange": false,
            "bFilter": false,
            "bInfo": false,
            "columnDefs": [
                {
                    "targets": 0,
                    "className": "text-center",
                    "orderable": false,
                    "data": null,
                    "order": [],
                    render: function (data, type, row, meta) {

                        return meta.row + page_size + 1;
                    }
                },
                {
                    "targets": [0, 1, 3, 4],
                    "searchable": false,
                    "orderable": false
                }],
            "order": [],
            "drawCallback": function (settings) {
                $('[data-toggle="tooltip"]').tooltip();
            },
        });
        t.on('order.dt search.dt', function () {
            t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + page_size + 1;
            });
        }).draw();
        reCalculatPagesCustom(rspn.count);
        viewBtnActionPage();
        hideLoading();
    } else if (rspn.data == "" || rspn.data == null || rspn.data == undefined) {
        var tbBody = $('#emailTypeTable tbody');
        $("#emailTypeTable").dataTable().fnDestroy();
        tbBody.html('');

        var page_size = (parseInt($("#txtCurrentPage").val()) - 1) * parseInt($("#cbPageSize").val())
        var t = $("#emailTypeTable").DataTable({
            "bPaginate": false,
            "bLengthChange": false,
            "bFilter": false,
            "bInfo": false,
            "columnDefs": [
                {
                    "targets": 0,
                    "className": "text-center",
                    "orderable": false,
                    "data": null,
                    "order": [],
                    render: function (data, type, row, meta) {

                        return meta.row + page_size + 1;
                    }
                },
                {
                    "targets": [0, 1, 3, 4],
                    "searchable": false,
                    "orderable": false
                }],
            "order": [],
            "drawCallback": function (settings) {
                $('[data-toggle="tooltip"]').tooltip();
            },
        });
        t.on('order.dt search.dt', function () {
            t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + page_size + 1;
            });
        }).draw();
        reCalculatPagesCustomNull();
        hideLoading();
    }
}
function onSearch() {
    var obj = {
        'email_address': $('#EmailAddress').val(),
        'page_size': parseInt($("#cbPageSize").val()),
        //'start_number': parseInt($("#txtCurrentPage").val())
        'start_number': (parseInt($("#txtCurrentPage").val()) - 1) * parseInt($("#cbPageSize").val())
    }
    callApi_userservice(
        apiConfig.api.sendmail.controller,
        apiConfig.api.sendmail.action.searchemail.path,
        apiConfig.api.sendmail.action.searchemail.method,
        obj, 'fnSearchDataSuccess', 'msgError');

    callApi_userservice(
        apiConfig.api.dataemail.controller,
        apiConfig.api.dataemail.action.dataemaildetail.path,
        apiConfig.api.dataemail.action.dataemaildetail.method,
        obj, 'fnDataEmailSuccess', 'msgError');
}
function fnDataEmailSuccess(rspn) {
    if (rspn.data!=null) {
        var frmDataEmail = $("#formDataEmail");
        frmDataEmail.find("#Subject").val(rspn.data.subject);
        rspn.data.check_auto == 1 ? $("#checkBoxStatus").prop('checked', true) : $("#checkBoxStatus").prop('checked', false);
        if (CKEDITOR.instances['BodyData']) {
            CKEDITOR.instances['BodyData'].setData(rspn.data.body);
            CKEDITOR.instances['BodyData'].updateElement();
        }
        EnableSave();
    }   
    //var frmDataEmail = $("#formDataEmail");
    //frmDataEmail.find("#Subject").val(rspn.data.subject);
    //rspn.data.check_auto == 1 ? $("#checkBoxStatus").prop('checked', true) : $("#checkBoxStatus").prop('checked', false);
    //if (CKEDITOR.instances['BodyData']) {
    //    CKEDITOR.instances['BodyData'].setData(rspn.data.body);
    //    CKEDITOR.instances['BodyData'].updateElement();
    //}
    //EnableSave();
}
function fnDeleteSuccess(rspn) {
    swal({
        title: "Thông báo",
        text: 'Bạn có chắc chắn muốn xoá bản ghi' + ' ' + '"' + rspn.data.email_address + '"',
        type: 'warning',
        showCancelButton: !0,
    }).then((isConfirm) => {
        if (isConfirm.value == true) {
            fnDeleteEmail(rspn.data.id);
        }
        return false;
    });
}
function Delete(id) {
    fnGetDetail(null, id);
}

function createEmailSuccess(data) {
    if (data.data !== null) {
        toastr.success("Thêm mới thành công");
        //localStorage.removeItem("id");
        //localStorage.removeItem("type");
        setTimeout(function () {
            openView(0, 0)
        }, 2000);
    }
    else {
        toastr.error(data.message)
        //setTimeout(function () { toastr.error(getStatusCode(data.code), 'Error', { progressBar: true }) }, 50);
    }
}

function fnDeleteEmailSuccess(rspn) {
    if (rspn.data === true) {
        toastr.success("Xóa dữ liệu thành công");
        onSearch();
    }
    else {
        toastr.error("Xóa dữ liệu thất bại");
    }
}



function fnDeleteEmail(id) {
    callApi_userservice(
        apiConfig.api.sendmail.controller,
        apiConfig.api.sendmail.action.deleteEmail.path + "?id=" + id,
        apiConfig.api.sendmail.action.deleteEmail.method,
        null, 'fnDeleteEmailSuccess', 'msgError');
}
function submitCreate() {
    var obj = {
        'email_address': $('#emailAddressCreate').val().trim(),
        'cc': $('#CCCreate').val().trim(),
    }
    if (validateRequired('#formCreate')) {
        callApi_userservice(
            apiConfig.api.sendmail.controller,
            apiConfig.api.sendmail.action.addEmail.path,
            apiConfig.api.sendmail.action.addEmail.method,
            obj, 'createEmailSuccess', 'msgError');
    }
}

function submitEdit() {
    var obj = {
        'Id': parseInt($('#IdEdit').val()),
        'email_address': $('#emailAddressEdit').val(),
        'cc': $('#CCEdit').val().trim(),
    }
    if (validateRequired('#formEdit')) {
        callApi_userservice(
            apiConfig.api.sendmail.controller,
            apiConfig.api.sendmail.action.updateEmail.path,
            apiConfig.api.sendmail.action.updateEmail.method,
            obj, 'updateEmailSuccess', 'msgError');
    }
}
function updateEmailSuccess(data) {
    if (data != false) {
        toastr.success("Cập nhật thành công");
        setTimeout(function () {
            openView(0, 0)
        }, 2000);
    }
    else {
        toastr.error("Cập nhật thất bại");
        //setTimeout(function () { toastr.error(getStatusCode(data.code), 'Error', { progressBar: true }) }, 70);
    }
}
function fnGetDetail(type, param) {
    var call_back = '';
    if (type === 3) {
        call_back = 'fnEditSuccess';
    }
    else if (type === 2) {
        call_back = 'fnGetDetailSuccess';
    }
    else {
        call_back = 'fnDeleteSuccess';
    }
    localStorage.removeItem("id");
    localStorage.removeItem("type");
    callApi_userservice(
        apiConfig.api.sendmail.controller,
        apiConfig.api.sendmail.action.getItem.path + "?id=" + param,
        apiConfig.api.sendmail.action.getItem.method,
        null, call_back, 'msgError');
}

function fnGetDetailSuccess(rspn) {
    var frmModify = $("#formEdit");
    var divVisible = $('#SaveEditBtn');
    divVisible.html('');
    if (rspn !== undefined && rspn !== null) {
        frmModify.find("#IdEdit").val(rspn.data.id);
        frmModify.find("#emailAddressEdit").val(rspn.data.email_address);
        frmModify.find("#CCEdit").val(rspn.data.cc);
    }
}
function fnEditSuccess(rspn) {
    localStorage.removeItem("id");
    localStorage.removeItem("type");
    var frmModify = $("#formEdit");
    var divVisible = $('#SaveEditBtn');
    divVisible.html('');
    var html =
        '<button type="button" class="btn btn-info" style="width: 100%" onclick="submitEdit();">' + "Lưu" +
        '</div >'
    divVisible.append(html);
    if (rspn !== undefined && rspn !== null) {
        frmModify.find("#IdEdit").val(rspn.data.id);
        frmModify.find("#emailAddressEdit").val(rspn.data.email_address);
        frmModify.find("#CCEdit").val(rspn.data.cc);
    }
}
function downloadSample() {
    showLoading();
    var obj = {
        'email_address': $('#EmailAddress').val().trim(),
        'page_size': parseInt($("#cbPageSize").val()),
        'start_number': (parseInt($("#txtCurrentPage").val()) - 1) * parseInt($("#cbPageSize").val())
    };
    var jsonData = JSON.stringify(obj);
    var request = new XMLHttpRequest();
    request.responseType = "blob";
    request.open("GET", apiConfig.api.host_user_service + apiConfig.api.sendmail.controller +
        apiConfig.api.sendmail.action.exportexcel.path + "?jsonData=" + jsonData);
    request.setRequestHeader('Authorization', getSessionToken());
    request.setRequestHeader('Accept-Language', 'vi-VN');
    request.onload = function () {
        hideLoading();
        if (this.status == 200) {
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(this.response);
            link.download = "Danh sách email.xlsx";
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
        if (this.status == 404) {
            toastr.error("Không tìm thấy dữ liệu!", "Lỗi!", { progressBar: true });
        }
        if (this.status == 400) {
            toastr.error("Có lỗi xảy ra!", "Lỗi!", { progressBar: true });
        }
    }
    request.send();
}
function SaveDataEmail() {
    var obj = {
        'subject': $('#Subject').val().trim(),
        'body': $.trim(CKEDITOR.instances["BodyData"].getData()),
        'checkauto': $('#checkBoxStatus').prop('checked') ? 1 : 0,
    }
    callApi_userservice(
        apiConfig.api.dataemail.controller,
        apiConfig.api.dataemail.action.savedataemail.path,
        apiConfig.api.dataemail.action.savedataemail.method,
        obj, 'SaveDataEmailSuccess', 'msgError');
    //if (validateRequired('#formDataEmail')) {

    //}
}
function SaveDataEmailSuccess(rspn) {
    if (rspn.data === true) {
        toastr.success("Thay đổi dữ liệu thành công");
    }
    else {
        toastr.error("Thay đổi dữ liệu thất bại");
    }
}
function EnableSave() {
    if ($('#Subject').val().length > 0) {
        return $("#SaveDataBtn").attr("disabled", false);
    } else return $("#SaveDataBtn").attr("disabled", true);
}
function ClearDataEmail() {
    $("#SaveDataBtn").attr("disabled", true);
    $('#Subject').val(null);
    CKEDITOR.instances['BodyData'].setData('');

}

function Start() //send mail
{
    var _objList = $('input[class="checkitem"]:checked').map((_, el) => el.value).get();
    var obj = {
        'Subject': $('#Subject').val().trim(),
        'Body': $.trim(CKEDITOR.instances["BodyData"].getData()),
        'To': _objList.join(),
    }
    callApi_userservice(
        apiConfig.api.sendmail.controller,
        apiConfig.api.sendmail.action.sendemail.path,
        apiConfig.api.sendmail.action.sendemail.method,
        obj, 'SendEmailSuccess', 'msgError');
}
function SendEmailSuccess(rspn) {
    if (rspn.data === true) {
        toastr.success(rspn.message);
    }
    else {
        toastr.error(rspn.message);
    }
}
function ImportFile() {

    if (validateRequired('#formView')) {
        var input = document.getElementById('fileToImport');
        if (input.files && input.files[0]) {
            var ext = $('#fileToImport').val().split('.').pop().toLowerCase();
            if ($.inArray(ext, ['xlsx', 'xls']) == -1) {
                toastr.error(localizationResources.ExcelAllow, "Error!", { progressBar: true });
                return;
            }
            var formData = new FormData();
            var imageFile = input.files[0];
            formData.append("file", imageFile);

            callUpload(apiConfig.api.sendmail.controller,
                apiConfig.api.sendmail.action.importexcel.path,
                formData, 'viewDataUpload', 'updateFail');
        }
    }

}
function viewDataUpload(rspn) {
    if (rspn.data === true) {
        toastr.success(rspn.message);
        onSearch();
    }
    else {
        toastr.error(rspn.message);
    }
}
