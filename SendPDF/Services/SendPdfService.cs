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
using DocumentFormat.OpenXml.Bibliography;

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
        public Task<bool> SendMailPDFAsync(DataSendMailPDF dataSendMailPDF, CurrentUserModel _userInfo)
        {
            try
            {
                List<DataFilePDF> listDataPDF = new List<DataFilePDF>();
                listDataPDF = (List<DataFilePDF>)(dataSendMailPDF.Data = DtPdf);
                byte[] filePDFs;
                var month = DateTime.Now.Month;
                var year = DateTime.Now.Year;
                var monthPC = 0;
                var yearPC = 0;
                if (month == 1)
                {
                    monthPC = 12;
                    yearPC = year - 1;
                }
                else
                {
                    monthPC = month - 1;
                    yearPC = year;
                }
                #region SendEmail
                var smtpClient = new SmtpClient(_configuration.GetSection("EmailHost").Value)
                {
                    Port = 587,
                    Credentials = new NetworkCredential((!String.IsNullOrEmpty(_userInfo.Email) && !String.IsNullOrEmpty(_userInfo.EmailPassword)) ? _userInfo.Email : _configuration.GetSection("EmailUsername").Value, (!String.IsNullOrEmpty(_userInfo.Email) && !String.IsNullOrEmpty(_userInfo.EmailPassword)) ?_userInfo.EmailPassword : _configuration.GetSection("EmailPassword").Value),
                    EnableSsl = true,
                };
                if (listDataPDF.Count > 0)
                {
                    for (int i = 0; i < listDataPDF.Count; i++)
                    {
                        #region List File PDF
                        var filepath = Path.Combine(_contentFolderFilePDF, "SampleFilePDF.pdf");
                        //using (PdfDocument doc = new PdfDocument(filepath))
                        //{
                        PdfDocument doc = new PdfDocument();

                        PdfSection sec = doc.Sections.Add();
                        sec.PageSettings.Width = PdfPageSize.A4.Width;
                        PdfPageBase page = sec.Pages.Add();
                        float y = 10;
                        PdfBrush brush1 = PdfBrushes.Black;
                        System.Drawing.Font font = new System.Drawing.Font("TimeNewRoman", 12f);
                        PdfTrueTypeFont trueTypeFont = new PdfTrueTypeFont(font, true);
                        PdfStringFormat format1 = new PdfStringFormat(PdfTextAlignment.Center);
                        PdfStringFormat format2 = new PdfStringFormat(PdfTextAlignment.Left);
                        
                        //Create a PdfTable object
                        PdfTable table = new PdfTable();
                        //Set font for header and the rest cells
                        table.Style.DefaultStyle.Font = new PdfTrueTypeFont(new Font("Times New Roman", 12f, FontStyle.Regular), true);
                        table.Style.HeaderStyle.Font = new PdfTrueTypeFont(new Font("Times New Roman", 12f, FontStyle.Bold), true);
                        
                        //Crate a DataTable
                        DataTable dataTable = new DataTable();
                        //if (month == 1)
                        //{
                            #region T1
                            page.Canvas.DrawString("Thông báo các khoản thu nhập chịu thuế tháng " + month + "/" + year, trueTypeFont, brush1, page.Canvas.ClientSize.Width / 2, y, format1);
                            page.Canvas.DrawString("\nĐơn vị: Tài chính kế toán", trueTypeFont, brush1, 0, y + 20, format2);
                            page.Canvas.DrawString("\nHọ tên: " + listDataPDF[i].EmployeeName + "\n", trueTypeFont, brush1, 0, y + 40, format2);
                            page.Canvas.DrawString("\nMã NV: " + listDataPDF[i].EmployeeCode + "\n", trueTypeFont, brush1, 0, y + 60, format2);
                            page.Canvas.DrawString("\nEmail: " + (!String.IsNullOrEmpty(_userInfo.Email) ? _userInfo.Email : _configuration.GetSection("EmailUsername").Value) + "\n", trueTypeFont, brush1, 0, y + 80, format2);
                            y = y + trueTypeFont.MeasureString("Country List", format1).Height;
                            y = y + 30;


                            dataTable.Columns.Add("Mục");
                            dataTable.Columns.Add("Nội dung");
                            dataTable.Columns.Add("Giá trị (vnđ)");
                            dataTable.Rows.Add(new string[] { "1", "Lương và các khoản phụ cấp T" + monthPC + "/" + yearPC, (listDataPDF[i].allowance == "" || listDataPDF[i].allowance == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].allowance)) });
                            dataTable.Rows.Add(new string[] { "2", "TNTT T" + monthPC + "/" + yearPC, (listDataPDF[i].TNTT == "" || listDataPDF[i].TNTT == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].TNTT)) });

                            dataTable.Rows.Add(new string[] { "3", "NCLĐ T" + monthPC + "/" + yearPC, (listDataPDF[i].NCLD == "" || listDataPDF[i].NCLD == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].NCLD)) });

                            dataTable.Rows.Add(new string[] { "4", "Hội nghị VC ", (listDataPDF[i].ConferenceVC == "" || listDataPDF[i].ConferenceVC == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].ConferenceVC)) });

                            dataTable.Rows.Add(new string[] { "5", "Tết nguyên đán" + yearPC, (listDataPDF[i].TND == "" || listDataPDF[i].TND == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].TND)) });

                            dataTable.Rows.Add(new string[] { "6", "Gặp mặt đầu xuân 27/01 ", (listDataPDF[i].GMDX == "" || listDataPDF[i].GMDX == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].GMDX)) });

                            dataTable.Rows.Add(new string[] { "7", "Chi bổ sung thu nhập năm " + yearPC, (listDataPDF[i].AdditionalExpenses == "" || listDataPDF[i].AdditionalExpenses == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].AdditionalExpenses)) });

                            dataTable.Rows.Add(new string[] { "8", "Thu hồi bổ sung TN năm " + yearPC, (listDataPDF[i].AdditionalRecall == "" || listDataPDF[i].AdditionalRecall == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].AdditionalRecall)) });

                            dataTable.Rows.Add(new string[] { "9", "DV khám chọn, khám yêu cầu T" + monthPC + "/" + yearPC, (listDataPDF[i].ServiceExamination == "" || listDataPDF[i].ServiceExamination == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].ServiceExamination)) });

                            dataTable.Rows.Add(new string[] { "10", "Trích nhà thuốc ", (listDataPDF[i].TNT == "" || listDataPDF[i].TNT == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].TNT)) });

                            dataTable.Rows.Add(new string[] { "11", "Tiền giảng viên lớp tập huấn ", (listDataPDF[i].TrainingClassTeacherMoney == "" || listDataPDF[i].TrainingClassTeacherMoney == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].TrainingClassTeacherMoney)) });

                            dataTable.Rows.Add(new string[] { "12", "Tiền thứ 7 tháng " + monthPC + "/" + yearPC, (listDataPDF[i].MoneySAT == "" || listDataPDF[i].MoneySAT == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].MoneySAT)) });

                            dataTable.Rows.Add(new string[] { "13", "Tiền chủ nhật tháng " + monthPC + "/" + yearPC, (listDataPDF[i].MoneySUN == "" || listDataPDF[i].MoneySUN == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].MoneySUN)) });

                            dataTable.Rows.Add(new string[] { "14", "Tiền làm thêm giờ (Khoa DDLS&TC) ", (listDataPDF[i].OvertimePay == "" || listDataPDF[i].OvertimePay == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].OvertimePay)) });

                            dataTable.Rows.Add(new string[] { "15", "Mổ yêu cầu ", (listDataPDF[i].RequestSurgery == "" || listDataPDF[i].RequestSurgery == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].RequestSurgery)) });

                            dataTable.Rows.Add(new string[] { "16", "DV 24/7 ", (listDataPDF[i].Dv247 == "" || listDataPDF[i].Dv247 == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].Dv247)) });

                            dataTable.Rows.Add(new string[] { "17", "Kỹ thuật dịch vụ mổ tự nguyện ", (listDataPDF[i].VoluntarySurgery == "" || listDataPDF[i].VoluntarySurgery == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].VoluntarySurgery)) });

                            dataTable.Rows.Add(new string[] { "18", "DV giảm đau sau mổ ", (listDataPDF[i].DvSurgeryPainRelief == "" || listDataPDF[i].DvSurgeryPainRelief == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].DvSurgeryPainRelief)) });

                            dataTable.Rows.Add(new string[] { "19", "PC điện thoại", (listDataPDF[i].PCDT == "" || listDataPDF[i].PCDT == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].PCDT)) });

                            dataTable.Rows.Add(new string[] { "20", "Các loại PC khác  (xăng xe, công tác phí) ", (listDataPDF[i].PCK == "" || listDataPDF[i].PCK == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].PCK)) });

                            dataTable.Rows.Add(new string[] { "21", "Tổng thu nhập", (listDataPDF[i].TotalIncome == "" || listDataPDF[i].TotalIncome == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].TotalIncome)) });

                            dataTable.Rows.Add(new string[] { "22", "Chênh lệch tiền ăn tính thuế (*)", (listDataPDF[i].CLTATT == "" || listDataPDF[i].CLTATT == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].CLTATT)) });

                            dataTable.Rows.Add(new string[] { "23", "Phụ cấp ưu đãi nghề", (listDataPDF[i].PCUDN == "" || listDataPDF[i].PCUDN == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].PCUDN)) });

                            dataTable.Rows.Add(new string[] { "24", "PC độc hại, PC khu vực", (listDataPDF[i].PCDH == "" || listDataPDF[i].PCDH == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].PCDH)) });

                            dataTable.Rows.Add(new string[] { "25", "Tổng thu nhập chịu thuế (21)-(3)+(22)-(23)-(24)", (listDataPDF[i].TotalTaxableIncome == "" || listDataPDF[i].TotalTaxableIncome == "0") ? "" : string.Format("{0:n0}", Int64.Parse(listDataPDF[i].TotalTaxableIncome)) });

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
                            table.Columns[0].Width = 3;
                            table.Columns[1].Width = 20;
                            table.Columns[2].Width = 5;
                            //Register with BeginRowLayout event
                            table.BeginRowLayout += Table_BeginRowLayout;
                            //Draw table on the page
                            table.Draw(page, new PointF(0, y + 80));
                            page.Canvas.DrawString("\n(*) Là phần chênh lệch giữa NCLĐ trong tháng với số được giảm trừ khi tính thuế TNCN\r\ntheo quy định (730.000 đồng cho tổng số ngày công thực tế trong tháng)", trueTypeFont, brush1, 0, y + 600, format2);

                        #endregion
                        //}
                        //else
                        //{
                        //    #region T2-12
                        //    page.Canvas.DrawString("Thông báo lương, TNTT, NCLĐ tháng " + month + "/" + year, trueTypeFont, brush1, page.Canvas.ClientSize.Width / 2, y, format1);
                        //    page.Canvas.DrawString("\nĐơn vị: Tài chính kế toán", trueTypeFont, brush1, 0, y + 20, format2);
                        //    page.Canvas.DrawString("\nHọ tên: " + listDataPDF[i].EmployeeName + "\n", trueTypeFont, brush1, 0, y + 40, format2);
                        //    page.Canvas.DrawString("\nMã NV: " + listDataPDF[i].EmployeeCode + "\n", trueTypeFont, brush1, 0, y + 60, format2);
                        //    page.Canvas.DrawString("\nEmail: " + (!String.IsNullOrEmpty(_userInfo.Email) ? _userInfo.Email : _configuration.GetSection("EmailUsername").Value) + "\n", trueTypeFont, brush1, 0, y + 80, format2);
                        //    y = y + trueTypeFont.MeasureString("Country List", format1).Height;
                        //    y = y + 30;

                        //    dataTable.Columns.Add("Mục");
                        //    dataTable.Columns.Add("Nội dung");
                        //    dataTable.Columns.Add("Giá trị (vnđ)");
                        //    #endregion
                        //}

                        doc.SaveToFile(filepath);
                        filePDFs = File.ReadAllBytes(filepath);

                        //}
                        #endregion

                        //var attachment = new Attachment(new MemoryStream(filePDFs), "test.pdf", "application/pdf");
                        //Attachment data = attachment;
                        var AddressEmailTo = listDataPDF[i].EmailAddress;
                        MailMessage mailMessage = new MailMessage();
                        mailMessage.Attachments.Add(new Attachment(new MemoryStream(filePDFs), "Thông báo các khoản thu nhập chịu thuế tháng " + month + "/" + year + ".pdf", "application/pdf"));
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
