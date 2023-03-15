namespace SendMailPDF.Common
{
    public class ResultModel
    {
        public string? Token { get; set; }
        public string? Message { get; set; }
        public int Code { get; set; }
        public object? Data { get; set; }
        public int Count { get; set; }
    }
    public class ResultImportModel
    {
        public string? Message { get; set; }
        public int Code { get; set; }
        public object? Data { get; set; }
        public int Count { get; set; }
    }
}
