using Microsoft.AspNetCore.Mvc;
using SendMailPDF.Attributes;
using SendMailPDF.Common;
using SendMailPDF.Models;
using SendMailPDF.Services;
using SendMailPDF.Services.Interface;

namespace SendMailPDF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BaseAuthorize]
    public class DataEmailController : Controller
    {
        private readonly ILogger<DataEmailController> _logger;
        protected readonly IConfiguration _config;
        private readonly ISendMailService _sendMailService;
        public DataEmailController(ILogger<DataEmailController> logger, IConfiguration config, ISendMailService sendMailService)
        {
            _logger = logger;
            _config = config;
            _sendMailService = sendMailService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("SaveDataEmail")]
        public async Task<ResultModel> SaveDataEmail([FromBody] DataEmailModel dataEmailModel)
        {
            try
            {
                if (HttpContext.Items["UserInfo"] is not CurrentUserModel _userInfo)
                {
                    return ResUnAuthorized.Unauthor();
                }
                return await _sendMailService.SaveDataEmailS(dataEmailModel, _userInfo);
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
            }
        }
        [HttpGet]
        [Route("DataEmailDetail")]
        public async Task<ResultModel> DataEmailDetail()
        {
            try
            {
                if (HttpContext.Items["UserInfo"] is not CurrentUserModel _userInfo)
                {
                    return ResUnAuthorized.Unauthor();
                }
                return await _sendMailService.DataEmailDetailS();
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
            }
        }
    }
}
