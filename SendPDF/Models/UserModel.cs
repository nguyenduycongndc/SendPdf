using System.Text.Json.Serialization;

namespace SendMailPDF.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int IsActive { get; set; }
    }
    public class SearchUserModel
    {
        [JsonPropertyName("user_name")]
        public string? UserName { get; set; }
        [JsonPropertyName("is_active")]
        public int IsActive { get; set; }
        [JsonPropertyName("start_number")]
        public int StartNumber { get; set; }
        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }
    }
    public class CurrentUserModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public int IsActive { get; set; }
        public string? Email { get; set; }
        public string? EmailPassword { get; set; }
        public int? RoleId { get; set; }
    }
    public class CreateModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? EmailAddress { get; set; }
        public int RoleId { get; set; }
    }
    public class LoginModel
    {
        //public string UserName { get; set; }

        public string Token { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
    }
    public class InputLoginModel
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
    public class UserCreateModel
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string PassWord { get; set; }
        public string SaltKey { get; set; }
        public int RoleId { get; set; }
        public int CreatedBy { get; set; }
    }

    public class OutModel
    {
        public string UserName { get; set; }

        public string PassWord { get; set; }
    }
    public class UpdateModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int IsActive { get; set; }
        public string FullName { get; set; }
    }
    public class UserUpdateModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int IsActive { get; set; }
        public string FullName { get; set; }
        public int ModifiedBy { get; set; }

    }
    public class UserUpdateOTPModel
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public DateTime Expdate { get; set; }
    }
    public class checkOTPModel
    {
        public string Email { get; set; }
        public string OTP { get; set; }
    }
    public class ForgotPassWordModel
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public string NewPassWord { get; set; }
        public string ConfirmPassWord { get; set; }
    }
    public class ChangePassWordModel
    {
        public string Email { get; set; }
        public string NewPassWord { get; set; }
        public string SaltKey { get; set; }
    }
    public class EmailModel
    {
        public string? Email { get; set; }
    }

    public class ExportUserModel
    {
        public List<UserModel> Data { get; set; }
        public int Count { get; set; }
        public string? Message { get; set; }
        public int Code { get; set; }
    }

    public class ChangePassWordLoginModel
    {
        public int Id { get; set; }
        public string PassWordOld { get; set; }
        public string PassWordNew { get; set; }
        public string ConfirmPassWordNew { get; set; }
    }
    public class ChangePassWordLoginSuccessModel
    {
        public int Id { get; set; }
        public string PassWordNew { get; set; }
        public string SaltKey { get; set; }
    }
    public class EditInformationModel
    {
        public int Id { get; set; }
        public string EmailAddressInstruct { get; set; }
        public string EmailPassword { get; set; }
    }
    public class UserEditInformationModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string EmailPassword { get; set; }
        public int ModifiedBy { get; set; }

    }
}
