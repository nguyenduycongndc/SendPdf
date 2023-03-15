using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SendMailPDF.Attributes;
using SendMailPDF.Services.Interface;
using SendMailPDF.Common;
using SendMailPDF.Services;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using SendMailPDF.Models;

namespace SendMailPDF.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //[Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginService _loginServices;
        private readonly ISendMailService _sendMailService;

        public LoginController(ILogger<LoginController> logger, ILoginService loginServices, ISendMailService sendMailService)
        {
            _logger = logger;
            _loginServices = loginServices;
            _sendMailService = sendMailService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("LoginUser")]
        public ResultModel LoginUser([FromBody] InputLoginModel inputModel)
        {
            var _login = _loginServices.Login(inputModel);
            if (!string.IsNullOrEmpty(_login.Token))
            {
                HttpContext.Session.SetString("SessionToken", _login.Token);
            }
            return _login;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/Login");
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("SendOTP")]
        public ResultModel SendOTP(string email)
        {
            try
            {
                var sendMailRs = _sendMailService.SendMailOTPAsync(email);
                if (sendMailRs == true)
                {
                    var data = new ResultModel()
                    {
                        Data = true,
                        Message = "Ok",
                        Code = 200,
                    };
                    return data;
                }
                else
                {
                    var data = new ResultModel()
                    {
                        Message = "Not Found",
                        Code = 404,
                    };
                    return data;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var data = new ResultModel()
                {
                    Message = "Not Found",
                    Code = 404,
                };
                return data;
                throw;
            }


        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ForgotPassWord")]
        public async Task<ResultModel> ForgotPassWord([FromBody] ForgotPassWordModel forgotPassWordModel)
        {
            try
            {
                var sendMailRs = await _loginServices.ForgotPassWordAsync(forgotPassWordModel);
                if (sendMailRs != null && sendMailRs.Code == 200)
                {
                    var data = new ResultModel()
                    {
                        Data = true,
                        Message = "Ok",
                        Code = 200,
                    };
                    return data;
                }
                else
                {
                    var data = new ResultModel()
                    {
                        Data = sendMailRs.Data,
                        Message = sendMailRs.Message,
                        Code = sendMailRs.Code,
                    };
                    return data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var data = new ResultModel()
                {
                    Message = "Not Found",
                    Code = 404,
                };
                return data;
                throw;
            }


        }
    }
}
