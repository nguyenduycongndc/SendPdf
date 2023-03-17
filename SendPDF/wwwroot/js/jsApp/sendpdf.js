function ImportFileExcelPDF() {
    var input = document.getElementById('fileToImportExcelPDF');
    if (input.files && input.files[0]) {
        var ext = $('#fileToImportExcelPDF').val().split('.').pop().toLowerCase();
        if ($.inArray(ext, ['xlsx', 'xls']) == -1) {
            toastr.error(localizationResources.ExcelAllow, "Error!", { progressBar: true });
            return;
        }
        var formData = new FormData();
        var imageFile = input.files[0];
        formData.append("file", imageFile);

        callUpload(apiConfig.api.sendfilepdf.controller,
            apiConfig.api.sendfilepdf.action.importexcelpdf.path,
            formData, 'viewDataUpload', 'updateFail');
    }
}

function viewDataUpload(rspn) {
    if (rspn.data === true) {
        toastr.success(rspn.message);
    }
    else {
        toastr.error(rspn.message);
    }
}

function SendEmailPDF() //send mail
{
    var obj = {
        'Subject': $('#Subject').val().trim(),
        'Body': $.trim(CKEDITOR.instances["BodyData"].getData()),
        'Data': {},
    }
    if (validateRequired('#formViewPdf')) {
        callApi_userservice(
            apiConfig.api.sendfilepdf.controller,
            apiConfig.api.sendfilepdf.action.sendpdf.path,
            apiConfig.api.sendfilepdf.action.sendpdf.method,
            obj, 'SendEmailPDFSuccess', 'msgError');
    }
}

function SendEmailPDFSuccess(rspn) {
    if (rspn.data === true) {
        toastr.success(rspn.message);
    }
    else {
        toastr.error(rspn.message);
    }
}
//function SaveDataEmailPDF() {
//    var obj = {
//        'subject': $('#Subject').val().trim(),
//        'body': $.trim(CKEDITOR.instances["BodyData"].getData()),
//        'checkauto': $('#checkBoxStatus').prop('checked') ? 1 : 0,
//    }
//    callApi_userservice(
//        apiConfig.api.dataemail.controller,
//        apiConfig.api.dataemail.action.savedataemail.path,
//        apiConfig.api.dataemail.action.savedataemail.method,
//        obj, 'SaveDataEmailSuccess', 'msgError');
//}

function ClearDataEmailPDF() {
    $("#sendMailBtnPDF").attr("disabled", true);
    $('#Subject').val(null);
    $('#fileToImportExcelPDF').val(null);
    CKEDITOR.instances['BodyData'].setData('');

}