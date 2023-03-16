using Microsoft.Extensions.Configuration;
using SendMailPDF.Models;
using System.Net.Mail;
using System.Net;
using SendMailPDF.Services.Interface;
using SendMailPDF.Common;
using SendMailPDF.Repo.Interface;
using SendMailPDF.Repo;

namespace SendMailPDF.Services
{
    public class SendPdfService: ISendPdfService
    {
        private readonly ILogger<SendPdfService> _logger;
        private readonly IConfiguration _configuration;
        private ResultModel Result;
        public SendPdfService(IConfiguration configuration, ILogger<SendPdfService> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }
        #region Send Mail
        public Task<bool> SendMailPDFAsync(DataSendMailPDF dataSendMailPDF)
        {
            try
            {
                var listEmail = dataSendMailPDF.Data;
                var smtpClient = new SmtpClient(_configuration.GetSection("EmailHost").Value)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_configuration.GetSection("EmailUsername").Value, _configuration.GetSection("EmailPassword").Value),
                    EnableSsl = true,
                };
                //if (listEmail.Length > 1)
                //{
                //    for (int i = 0; i < listEmail.Length; i++)
                //    {
                //        var AddressEmailTo = listEmail[i].Split(";");
                //        MailMessage mailMessage = new MailMessage();
                //        mailMessage.Subject = dataSendMailPDF.Subject;
                //        mailMessage.Body = dataSendMailPDF.Body;
                //        mailMessage.From = new MailAddress(_configuration.GetSection("EmailUsername").Value);
                //        mailMessage.To.Add(new MailAddress(AddressEmailTo[0]));
                //        if (AddressEmailTo[1] != "")
                //        {
                //            mailMessage.CC.Add(new MailAddress(AddressEmailTo[1]));
                //        }
                //        mailMessage.IsBodyHtml = true;
                //        smtpClient.Send(mailMessage);
                //    }
                //}
                //else
                //{
                //    var AddressEmailTo = dataSendMailPDF.To.Split(";");
                //    MailMessage mailMessage = new MailMessage();
                //    mailMessage.Subject = dataSendMailPDF.Subject;
                //    mailMessage.Body = dataSendMailPDF.Body;
                //    mailMessage.From = new MailAddress(_configuration.GetSection("EmailUsername").Value);
                //    mailMessage.To.Add(new MailAddress(AddressEmailTo[0]));
                //    if (AddressEmailTo[1] != "")
                //    {
                //        mailMessage.CC.Add(new MailAddress(AddressEmailTo[1]));
                //    }
                //    mailMessage.IsBodyHtml = true;
                //    smtpClient.Send(mailMessage);
                //}
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
                throw;
            }
        }
        #endregion
    }
}
