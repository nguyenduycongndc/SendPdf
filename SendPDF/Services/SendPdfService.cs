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
        private static List<DataFilePDF> DtPdf;
        public SendPdfService(IConfiguration configuration, ILogger<SendPdfService> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public Task<bool> ImportPDF(List<DataFilePDF> dataFilePDF)
        {
            try
            {
                DtPdf = dataFilePDF;
                return Task.FromResult(true);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region Send Mail
        public Task<bool> SendMailPDFAsync(DataSendMailPDF dataSendMailPDF)
        {
            try
            {
                List<DataFilePDF> listEmail = new List<DataFilePDF>();
                listEmail = (List<DataFilePDF>)(dataSendMailPDF.Data = DtPdf);

                #region File PDF

                #endregion
                #region SendEmail
                var smtpClient = new SmtpClient(_configuration.GetSection("EmailHost").Value)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_configuration.GetSection("EmailUsername").Value, _configuration.GetSection("EmailPassword").Value),
                    EnableSsl = true,
                };
                if (listEmail.Count > 1)
                {
                    for (int i = 0; i < listEmail.Count; i++)
                    {
                        var AddressEmailTo = listEmail[i].EmailAddress;
                        MailMessage mailMessage = new MailMessage();
                        mailMessage.Subject = dataSendMailPDF.Subject;
                        mailMessage.Body = dataSendMailPDF.Body;
                        mailMessage.From = new MailAddress(_configuration.GetSection("EmailUsername").Value);
                        mailMessage.To.Add(new MailAddress(AddressEmailTo));
                        mailMessage.IsBodyHtml = true;
                        smtpClient.Send(mailMessage);
                    }
                }
                #endregion
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
