using System.Text.Json.Serialization;

namespace SendMailPDF.Models
{
    public class EmailDto
    {
        public string To { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
    public class EmailSearchModel
    {
        public string? email_address { get; set; }
        [JsonPropertyName("start_number")]
        public int StartNumber { get; set; }
        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }
    }
    public class EmailDataModel
    {
        public int Id { get; set; }
        public string? email_address { get; set; }
        public string? cc { get; set; }
        public string? create_at { get; set; }
    }
    public class CreateEmailModel
    {
        public string email_address { get; set; }
        public string? cc { get; set; }
    }
    public class EmailCrModel
    {
        public string email_address { get; set; }
        public string? cc { get; set; }
    }

    public class EmailDeModel
    {
        public int Id { get; set; }
        public string email_address { get; set; }
        public string? cc { get; set; }
    }
    public class DataEmailModel
    {
        public string subject { get; set; }
        public string? body { get; set; }
        public int? checkauto { get; set; }
    }
    public class EmailUpdateModel
    {
        public int Id { get; set; }
        public string email_address { get; set; }
        public string? cc { get; set; }
    }
    public class EmailUpModel
    {
        public int Id { get; set; }
        public string email_address { get; set; }
        public string? cc { get; set; }
    }
}
