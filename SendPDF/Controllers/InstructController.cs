using Microsoft.AspNetCore.Mvc;
using SendMailPDF.Attributes;
using SendMailPDF.Models;
using SendPDF.Controllers;

namespace SendMailPDF.Controllers
{
    [Route("[controller]")]
    [BaseAuthorize]
    public class InstructController : Controller
    {
        private readonly ILogger<InstructController> _logger;

        public InstructController(ILogger<InstructController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Items["UserInfo"] is not CurrentUserModel _userInfo)
            {
                return Unauthorized();
            }
            return View();
        }
    }
}
