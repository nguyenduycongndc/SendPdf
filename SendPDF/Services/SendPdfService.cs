using Microsoft.Extensions.Configuration;
using SendMailPDF.Models;
using System.Net.Mail;
using System.Net;
using SendMailPDF.Services.Interface;
using SendMailPDF.Common;
using SendMailPDF.Repo.Interface;
using SendMailPDF.Repo;
using Microsoft.AspNetCore.Hosting;
using Spire.Pdf;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Pdf.Graphics;
using Spire.Pdf.Tables;
using System.Drawing;
using System.Data;

namespace SendMailPDF.Services
{
    public class SendPdfService : ISendPdfService
    {
        private readonly ILogger<SendPdfService> _logger;
        private readonly IConfiguration _configuration;
        private static List<DataFilePDF> DtPdf;
        public readonly string _contentFolderFilePDF;
        public const string CONTEN_FOLDER_NAME_FILE_PDF = "SampleFile";
        public SendPdfService(IConfiguration configuration, ILogger<SendPdfService> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _configuration = configuration;
            _contentFolderFilePDF = Path.Combine(webHostEnvironment.WebRootPath, CONTEN_FOLDER_NAME_FILE_PDF);
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
        private static void Table_BeginRowLayout(object sender, BeginRowLayoutEventArgs args)
        {
            //Set row height
            args.MinimalHeight = 20f;
            //Alternate row color
            if (args.RowIndex < 0)
            {
                return;
            }
            if (args.RowIndex % 2 == 1)
            {
                args.CellStyle.BackgroundBrush = PdfBrushes.LightGray;
            }
            else
            {
                args.CellStyle.BackgroundBrush = PdfBrushes.White;
            }
        }

        #region Send Mail
        public Task<bool> SendMailPDFAsync(DataSendMailPDF dataSendMailPDF)
        {
            try
            {
                List<DataFilePDF> listDataPDF = new List<DataFilePDF>();
                listDataPDF = (List<DataFilePDF>)(dataSendMailPDF.Data = DtPdf);
                byte[] filePDFs;


                #region SendEmail
                var smtpClient = new SmtpClient(_configuration.GetSection("EmailHost").Value)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_configuration.GetSection("EmailUsername").Value, _configuration.GetSection("EmailPassword").Value),
                    EnableSsl = true,
                };
                if (listDataPDF.Count > 1)
                {
                    for (int i = 0; i < listDataPDF.Count; i++)
                    {
                        #region List File PDF
                        var filepath = Path.Combine(_contentFolderFilePDF, "SampleFilePDF.pdf");
                        //using (PdfDocument doc = new PdfDocument(filepath))
                        //{
                        PdfDocument doc = new PdfDocument();

                        PdfPageBase page = doc.Pages.Add(PdfPageSize.A4, new PdfMargins(40));

                        //Create a PdfTable object
                        PdfTable table = new PdfTable();
                        //Set font for header and the rest cells
                        table.Style.DefaultStyle.Font = new PdfTrueTypeFont(new Font("Times New Roman", 12f, FontStyle.Regular), true);
                        table.Style.HeaderStyle.Font = new PdfTrueTypeFont(new Font("Times New Roman", 12f, FontStyle.Bold), true);

                        //Crate a DataTable
                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("ID");
                        dataTable.Columns.Add("Name");
                        dataTable.Columns.Add("Department");
                        dataTable.Columns.Add("Position");
                        dataTable.Columns.Add("Level");
                        dataTable.Rows.Add(new string[] { "1", "David", "IT", "Manager", "1" });
                        dataTable.Rows.Add(new string[] { "3", "Julia", "HR", "Manager", "1" });
                        dataTable.Rows.Add(new string[] { "4", "Sophie", "Marketing", "Manager", "1" });
                        dataTable.Rows.Add(new string[] { "7", "Wickey", "Marketing", "Sales Rep", "2" });
                        dataTable.Rows.Add(new string[] { "9", "Wayne", "HR", "HR Supervisor", "2" });
                        dataTable.Rows.Add(new string[] { "11", "Mia", "Dev", "Developer", "2" });
                        //Set the datatable as the data source of table
                        table.DataSource = dataTable;

                        //Show header(the header is hidden by default)
                        table.Style.ShowHeader = true;
                        //Set font color and backgroud color of header row
                        table.Style.HeaderStyle.BackgroundBrush = PdfBrushes.Gray;
                        table.Style.HeaderStyle.TextBrush = PdfBrushes.White;
                        //Set text alignment in header row
                        table.Style.HeaderStyle.StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);


                        //Set text alignment in other cells
                        for (int a = 0; a < table.Columns.Count; a++)
                        {
                            table.Columns[a].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                        }
                        //Register with BeginRowLayout event
                        table.BeginRowLayout += Table_BeginRowLayout;
                        //Draw table on the page
                        table.Draw(page, new PointF(0, 30));

                        doc.SaveToFile(filepath);
                        filePDFs = File.ReadAllBytes(filepath);

                        //}
                        #endregion

                        //var attachment = new Attachment(new MemoryStream(filePDFs), "test.pdf", "application/pdf");
                        //Attachment data = attachment;
                        var AddressEmailTo = listDataPDF[i].EmailAddress;
                        MailMessage mailMessage = new MailMessage();
                        mailMessage.Attachments.Add(new Attachment(new MemoryStream(filePDFs), "test.pdf", "application/pdf"));
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
