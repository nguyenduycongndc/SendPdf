using SendMailPDF.Data;
using SendMailPDF.Models;

namespace SendMailPDF.Repo.Interface
{
    public interface IEmailRepo
    {
        Task<List<Email>> GetAllEmail(EmailSearchModel emailSearchModel);
        List<Email> CheckEmail(string email);
        Task<bool> CreateEmailR(EmailCrModel cre);
        List<Email> GetDetailEmailR(int id);
        Task<bool> DeleteEmailR(int id, CurrentUserModel _userInfo);
        Task<List<Email>> CheckAllEmail();
        Task<bool> UpdateEmail(EmailUpdateModel emailModel);
    }
}
