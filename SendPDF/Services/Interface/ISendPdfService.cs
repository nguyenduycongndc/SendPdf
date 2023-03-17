using SendMailPDF.Models;

namespace SendMailPDF.Services.Interface
{
    public interface ISendPdfService
    {
        Task<bool> SendMailPDFAsync(DataSendMailPDF dataSendMailPDF);
        public Task<bool> ImportPDF(List<DataFilePDF> dataSendMailPDF);
    }
}
