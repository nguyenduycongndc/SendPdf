using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SendMailPDF.Data
{
    [Table("email")]
    public class Email
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Column("email_address")]
        [JsonPropertyName("email_address")]
        public string? EmailAddress { get; set; }

        [Column("cc")]
        [JsonPropertyName("cc")]
        public string? CC { get; set; }

        [Column("is_deleted")]
        [JsonPropertyName("is_deleted")]
        public int? IsDeleted { get; set; }

        [Column("created_at")]
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("deleted_at")]
        [JsonPropertyName("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        [Column("deleted_by")]
        [JsonPropertyName("deleted_by")]
        public int? DeletedBy { get; set; }
    }
}
