using SendMailPDF.Data;
using SendMailPDF.Models;

namespace SendMailPDF.Repo.Interface
{
    public interface IUserRepo
    {
        Task<List<Users>> GetAll(SearchUserModel searchUserModel);
        //Task<bool> CreateUs(Users user, UsersRoles usersRoles);
        Task<bool> CreateUs(UserCreateModel us);
        List<Users> CheckUser(string userName);
        Task<List<Roles>> CheckRoles(int RolesId);
        List<Users> GetDetail(int id);
        List<Users> CheckEmailUser(string email);
        List<Users> CheckAllEmailUser();
        List<Users> CheckOTP(checkOTPModel checkOTPModel);
        Task<List<Users>> CheckAllUser();
        Task<bool> UpdateUs(UserUpdateModel user);
        Task<bool> UpdateOTPUs(UserUpdateOTPModel userUpdateOTPModel);
        Task<bool> DeleteUs(int id, CurrentUserModel _userInfo);
        Task<bool> ForgotPassWordUs(ChangePassWordModel changePassWordModel);
        Task<bool> ChangePassWordRepo(ChangePassWordLoginSuccessModel changePassWordLoginSuccessModel);

        //public Users GetDetail(int id);
        //Users GetDetailByName(InputLoginModel inputModel);
    }
}
