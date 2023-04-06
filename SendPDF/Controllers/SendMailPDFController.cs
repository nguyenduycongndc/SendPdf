using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SendMailPDF.Attributes;
using SendMailPDF.Common;
using SendMailPDF.Models;
using SendMailPDF.Services.Interface;

namespace SendMailPDF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BaseAuthorize]
    public class SendMailPDFController : Controller
    {
        private readonly ILogger<SendMailPDFController> _logger;
        protected readonly IConfiguration _config;
        private readonly ISendPdfService _sendPdfService;
        public readonly string _contentFolder;
        public const string CONTEN_FOLDER_NAME = "UploadFile";
        public static List<DataFilePDF> dataFilePDF = new List<DataFilePDF>();
        public SendMailPDFController(ILogger<SendMailPDFController> logger, IConfiguration config, IWebHostEnvironment webHostEnvironment, ISendPdfService sendPdfService)
        {
            _logger = logger;
            _config = config;
            _sendPdfService = sendPdfService;
            _contentFolder = Path.Combine(webHostEnvironment.WebRootPath, CONTEN_FOLDER_NAME);
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        //import excel
        [HttpPost]
        [Route("ImportExcelPDF"), DisableRequestSizeLimit]
        public async Task<ResultImportModel> ImportExcelPDF(IFormFile file)
        {
            try
            {
                if (HttpContext.Items["UserInfo"] is not CurrentUserModel _userInfo)
                {
                    return ResUnAuthorized.UnauthorImport();
                }
                var filepath = Path.Combine(_contentFolder, file.FileName);
                using (FileStream fs = System.IO.File.Create(filepath))
                {
                    file.CopyTo(fs);
                }
                int rowno = 1;
                XLWorkbook workbook = XLWorkbook.OpenFromTemplate(filepath);
                var sheets = workbook.Worksheets.First();
                var rows = sheets.Rows().ToList();
                var checkStatusT = new List<ResultModel>();
                var dataRs = new ResultImportModel();
                dataFilePDF.Clear();
                foreach (var row in rows)
                {
                    if (rowno > 1)
                    {
                        var nullRow = row.Cell(1).Value.ToString();
                        if (string.IsNullOrWhiteSpace(nullRow) || string.IsNullOrEmpty(nullRow))
                        {
                            break;
                        }
                        dataFilePDF.Add(new DataFilePDF()
                        {
                            No = row.Cell(1).Value.ToString(),
                            Departments = row.Cell(2).Value.ToString(),
                            EmployeeCode = row.Cell(3).Value.ToString(),
                            EmployeeName = row.Cell(4).Value.ToString(),
                            EmailAddress = row.Cell(5).Value.ToString(),
                            allowance = row.Cell(6).Value.ToString(),
                            TNTT = row.Cell(7).Value.ToString(),
                            NCLD = row.Cell(8).Value.ToString(),
                            ConferenceVC = row.Cell(9).Value.ToString(),
                            TND = row.Cell(10).Value.ToString(),
                            GMDX = row.Cell(11).Value.ToString(),
                            AdditionalExpenses = row.Cell(12).Value.ToString(),
                            AdditionalRecall = row.Cell(13).Value.ToString(),
                            ServiceExamination = row.Cell(14).Value.ToString(),
                            TNT = row.Cell(15).Value.ToString(),
                            TrainingClassTeacherMoney = row.Cell(16).Value.ToString(),
                            MoneySAT = row.Cell(17).Value.ToString(),
                            MoneySUN = row.Cell(18).Value.ToString(),
                            OvertimePay = row.Cell(19).Value.ToString(),
                            RequestSurgery = row.Cell(20).Value.ToString(),
                            Dv247 = row.Cell(21).Value.ToString(),
                            VoluntarySurgery = row.Cell(22).Value.ToString(),
                            DvSurgeryPainRelief = row.Cell(23).Value.ToString(),
                            PCDT = row.Cell(24).Value.ToString(),
                            PCK = row.Cell(25).Value.ToString(),
                            TotalIncome = row.Cell(26).Value.ToString(),
                            CLTATT = row.Cell(27).Value.ToString(),
                            PCUDN = row.Cell(28).Value.ToString(),
                            PCDH = row.Cell(29).Value.ToString(),
                            TotalTaxableIncome = row.Cell(30).Value.ToString(),
                            Insurance = row.Cell(31).Value.ToString(),
                            TemporarilyWithdrawn = row.Cell(32).Value.ToString(),
                        });
                    }
                    else
                    {
                        rowno++;
                    }
                }
                var success = await _sendPdfService.ImportPDF(dataFilePDF);
                if (success)
                {
                    dataRs = new ResultImportModel()
                    {
                        Message = "Success",
                        Code = 200,
                        Data = true,
                        Count = checkStatusT.Count,
                    };
                }
                else
                {
                    dataRs = new ResultImportModel()
                    {
                        Message = "Falid",
                        Code = 400,
                        Data = false,
                        Count = checkStatusT.Count,
                    };
                }
                return dataRs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [HttpPost]
        [Route("SendFilePDF")]
        public async Task<ResultModel> SendFilePDF([FromBody] DataSendMailPDF dataSendMailPDF)
        {
            try
            {
                if (HttpContext.Items["UserInfo"] is not CurrentUserModel _userInfo)
                {
                    return ResUnAuthorized.Unauthor();
                }
                var sendMailRs = await _sendPdfService.SendMailPDFAsync(dataSendMailPDF);
                if (sendMailRs == true)
                {
                    var data = new ResultModel()
                    {
                        Data = true,
                        Message = "Email sending success",
                        Code = 200,
                    };
                    return data;
                }
                else if (sendMailRs == false)
                {
                    var data = new ResultModel()
                    {
                        Data = false,
                        Message = "Email sending failed",
                        Code = 400,
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

    }
}
