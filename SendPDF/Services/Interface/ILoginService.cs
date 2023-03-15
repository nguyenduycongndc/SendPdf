using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendMailPDF.Common;
using SendMailPDF.Models;

namespace SendMailPDF.Services.Interface
{
    public interface ILoginService
    {
        public ResultModel Login(InputLoginModel inputModel);
        public Task<ResultModel> ForgotPassWordAsync(ForgotPassWordModel forgotPassWordModel);
    }
}
