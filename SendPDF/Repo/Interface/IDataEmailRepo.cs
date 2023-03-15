using SendMailPDF.Common;
using SendMailPDF.Data;
using SendMailPDF.Models;

namespace SendMailPDF.Repo.Interface
{
    public interface IDataEmailRepo
    {
        DataEmail CheckDataEmail();
        DataEmail CheckDataEmailAuto();
        Task<bool> CrUpDataEmail(DataEmailModel dataEmailModel, CurrentUserModel _userInfo);
        DataEmail DataEmailDetail();
        //Task<bool> UpdateDataEmail(DataEmailModel dataEmailModel, CurrentUserModel _userInfo);
    }
}
