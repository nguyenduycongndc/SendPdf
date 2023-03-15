using SendMailPDF.Common;
using SendMailPDF.Models;

namespace SendMailPDF.Services.Interface
{
    public interface ISendMailService
    {
        Task<bool> SendMailAsync(EmailDto emailDto);
        bool SendMailOTPAsync(string Email);
        Task<ResultModel> GetAllEmailService(EmailSearchModel emailSearchModel);
        public Task<ResultModel> CreateEmailS(CreateEmailModel createEmailModel);
        public ResultModel GetDetailEmailModels(int id);
        public Task<ResultModel> DeleteEmail(int id, CurrentUserModel _userInfo);
        public Task<ResultModel> SaveDataEmailS(DataEmailModel dataEmailModel, CurrentUserModel _userInfo);
        public Task<ResultModel> DataEmailDetailS();
        public Task<ResultModel> UpdateEmailS(EmailUpModel emailDeModel);
    }
}
