using DocumentFormat.OpenXml.Office2010.Excel;
using SendMailPDF.Common;
using SendMailPDF.Data;
using SendMailPDF.Models;
using SendMailPDF.Repo;
using SendMailPDF.Repo.Interface;
using SendMailPDF.Services.Interface;
using System;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Mail;
using System.Web.Helpers;

namespace SendMailPDF.Services
{
    public class SendMailService : ISendMailService
    {
        private readonly ILogger<SendMailService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserRepo _userRepo;
        private readonly IEmailRepo _emailRepo;
        private readonly IDataEmailRepo _dataemailRepo;
        private ResultModel Result;

        public SendMailService(IConfiguration configuration, ILogger<SendMailService> logger, IUserRepo userRepo, IEmailRepo emailRepo, IDataEmailRepo dataemailRepo)
        {
            _logger = logger;
            _configuration = configuration;
            _userRepo = userRepo;
            _emailRepo = emailRepo;
            _dataemailRepo = dataemailRepo;
        }
        #region Send Mail
        public Task<bool> SendMailAsync(EmailDto emailDto)
        {
            try
            {
                string[] listEmail = emailDto.To.Split(",");
                var smtpClient = new SmtpClient(_configuration.GetSection("EmailHost").Value)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_configuration.GetSection("EmailUsername").Value, _configuration.GetSection("EmailPassword").Value),
                    EnableSsl = true,
                };
                if (listEmail.Length > 1)
                {
                    for (int i = 0; i < listEmail.Length; i++)
                    {
                        var AddressEmailTo = listEmail[i].Split(";");
                        MailMessage mailMessage = new MailMessage();
                        mailMessage.Subject = emailDto.Subject;
                        mailMessage.Body = emailDto.Body;
                        mailMessage.From = new MailAddress(_configuration.GetSection("EmailUsername").Value);
                        mailMessage.To.Add(new MailAddress(AddressEmailTo[0]));
                        if (AddressEmailTo[1] != "")
                        {
                            mailMessage.CC.Add(new MailAddress(AddressEmailTo[1]));
                        }
                        mailMessage.IsBodyHtml = true;
                        smtpClient.Send(mailMessage);
                    }
                }
                else
                {
                    var AddressEmailTo = emailDto.To.Split(";");
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.Subject = emailDto.Subject;
                    mailMessage.Body = emailDto.Body;
                    mailMessage.From = new MailAddress(_configuration.GetSection("EmailUsername").Value);
                    mailMessage.To.Add(new MailAddress(AddressEmailTo[0]));
                    if (AddressEmailTo[1] != "")
                    {
                        mailMessage.CC.Add(new MailAddress(AddressEmailTo[1]));
                    }
                    mailMessage.IsBodyHtml = true;
                    smtpClient.Send(mailMessage);
                }
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
                throw;
            }
        }
        #endregion
        #region Send OTP
        public bool SendMailOTPAsync(string email)
        {
            try
            {
                string OTP = "";
                Random rand = new Random();
                OTP = rand.Next(0, 1000000).ToString("D6");
                var datenow = DateTime.Now;
                var expdate = datenow.AddMinutes(2);
                var rs = _userRepo.CheckEmailUser(email);
                if (rs.Count == 0)
                {
                    return false;
                }
                else
                {
                    var userUpdateOTP = new UserUpdateOTPModel()
                    {
                        Email = email,
                        OTP = OTP,
                        Expdate = expdate,
                    };
                    _userRepo.UpdateOTPUs(userUpdateOTP);
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.Subject = "OTP thay đổi mật khẩu";
                    mailMessage.Body = "Mã OTP của bạn là: " + OTP;
                    mailMessage.From = new MailAddress(_configuration.GetSection("EmailUsername").Value);
                    mailMessage.To.Add(new MailAddress(email));
                    mailMessage.IsBodyHtml = true;
                    var smtpClient = new SmtpClient(_configuration.GetSection("EmailHost").Value)
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(_configuration.GetSection("EmailUsername").Value, _configuration.GetSection("EmailPassword").Value),
                        EnableSsl = true,
                    };
                    smtpClient.Send(mailMessage);
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        #endregion
        #region Get All Email
        public async Task<ResultModel> GetAllEmailService(EmailSearchModel emailSearchModel)
        {
            try
            {
                var qr = await _emailRepo.GetAllEmail(emailSearchModel);
                var lst = await _emailRepo.CheckAllEmail();
                var listData =  qr.Select(x => new EmailDataModel()
                {
                    Id = x.Id,
                    email_address = x.EmailAddress,
                    cc = x.CC,
                    create_at = x.CreatedAt.HasValue ? x.CreatedAt.Value.ToString("dd/MM/yyyy") : null,
                }).OrderBy(x => x.Id).ToList();
                var data = new ResultModel()
                {
                    Data = listData,
                    Message = "Successfull",
                    Code = 200,
                    Count = lst.Count(),
                };
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Create
        public async Task<ResultModel> CreateEmailS(CreateEmailModel creaetEmailModel)
        {
            try
            {
                var checkEmail = _emailRepo.CheckEmail(creaetEmailModel.email_address);
                if (checkEmail.Count() > 0)
                {
                    _logger.LogError("Email này đã có trong hệ thống");
                    Result = new ResultModel()
                    {
                        Message = "Email này đã có trong hệ thống",
                        Code = 404,
                    };
                    return Result;
                }
                if (string.IsNullOrEmpty(creaetEmailModel.email_address))
                {
                    Result = new ResultModel()
                    {
                        Message = "Bad Request",
                        Code = 400,
                    };
                    return Result;
                }

                EmailCrModel Em = new EmailCrModel()
                {
                    email_address = creaetEmailModel.email_address,
                    cc = creaetEmailModel.cc,
                };
                var rs = await _emailRepo.CreateEmailR(Em);
                Result = new ResultModel()
                {
                    Data = rs,
                    Message = (rs == true ? "OK" : "Bad Request"),
                    Code = (rs == true ? 200 : 400),
                };
                return Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result;
            }
        }
        #endregion
        #region Update
        public async Task<ResultModel> UpdateEmailS(EmailUpModel emailDeModel)
        {
            try
            {
                var checkEmail = new List<Email>();
                var checkEmailDe = _emailRepo.GetDetailEmailR(emailDeModel.Id);
                if (string.IsNullOrEmpty(emailDeModel.email_address) && emailDeModel.email_address != checkEmailDe[0].EmailAddress)
                {
                    checkEmail = _emailRepo.CheckEmail(emailDeModel.email_address);
                    if (checkEmail.Count() != 0)
                    {
                        _logger.LogError("Email này đã được sử dụng");
                        Result = new ResultModel()
                        {
                            Message = "Not Found",
                            Code = 404,
                        };
                        return Result;
                    }
                }
                //if (checkEmail.Count() == 0)
                //{
                //    _logger.LogError("Tài khoản không tồn tại");
                //    Result = new ResultModel()
                //    {
                //        Message = "Not Found",
                //        Code = 404,
                //    };
                //    return Result;
                //}

                EmailUpdateModel us = new EmailUpdateModel()
                {
                    Id = emailDeModel.Id,
                    email_address = emailDeModel.email_address,
                    cc = emailDeModel.cc,
                };
                var rs = await _emailRepo.UpdateEmail(us);
                Result = new ResultModel()
                {
                    Data = rs,
                    Message = (rs == true ? "OK" : "Bad Request"),
                    Code = (rs == true ? 200 : 400),
                };
                return Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result;
            }
        }
        #endregion
        #region Delete
        public async Task<ResultModel> DeleteEmail(int id, CurrentUserModel _userInfo)
        {
            try
            {
                var checkUser = _emailRepo.GetDetailEmailR(id);
                if (checkUser.Count() == 0)
                {
                    _logger.LogError("Email này không tồn tại");
                    Result = new ResultModel()
                    {
                        Message = "Not Found",
                        Code = 404,
                    };
                    return Result;
                }
                var rs = await _emailRepo.DeleteEmailR(id, _userInfo);
                Result = new ResultModel()
                {
                    Data = rs,
                    Message = (rs == true ? "OK" : "Bad Request"),
                    Code = (rs == true ? 200 : 400),
                };
                return Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result;
            }
        }
        #endregion
        #region Save Data Email
        public async Task<ResultModel> SaveDataEmailS(DataEmailModel dataEmailModel, CurrentUserModel _userInfo)
        {
            try
            {
                var checkDataEmail = _dataemailRepo.CheckDataEmail();
                var rs = await _dataemailRepo.CrUpDataEmail(dataEmailModel, _userInfo);
                Result = new ResultModel()
                {
                    Data = rs,
                    Message = (rs != null ? "OK" : "Bad Request"),
                    Code = (rs != null ? 200 : 400),
                };
                return Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result;
            }
        }
        #endregion
        #region Detail 
        public ResultModel GetDetailEmailModels(int Id)
        {
            try
            {
                var rs = _emailRepo.GetDetailEmailR(Id);
                if (rs.Count == 0)
                {
                    return Result;
                }
                else
                {
                    var detailUs = new EmailDeModel()
                    {
                        Id = rs[0].Id,
                        email_address = rs[0].EmailAddress,
                        cc = rs[0].CC,
                    };

                    Result = new ResultModel()
                    {
                        Data = detailUs,
                        Message = "OK"/*"Successfull"*/,
                        Code = 200,
                    };

                    return Result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result;
            }
        }
        public async Task<ResultModel> DataEmailDetailS()
        {
            try
            {
                var checkDataEmail = _dataemailRepo.DataEmailDetail();
                //var rs = await _dataemailRepo.DataEmailDetail();
                Result = new ResultModel()
                {
                    Data = checkDataEmail,
                    Message = (checkDataEmail != null ? "OK" : "Bad Request"),
                    Code = (checkDataEmail != null ? 200 : 400),
                };
                return Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result;
            }
        }
        #endregion
    }
}
