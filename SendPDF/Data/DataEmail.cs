using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SendMailPDF.Data
{
    [Table("data_email")]
    public class DataEmail
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Column("subject")]
        [JsonPropertyName("subject")]
        [StringLength(100)]
        public string? Subject { get; set; }

        [Column("body")]
        [JsonPropertyName("body")]
        [StringLength(3000)]
        public string? Body { get; set; }

        [Column("created_by")]
        [JsonPropertyName("created_by")]
        public int? CreatedBy { get; set; }

        [Column("created_at")]
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("check_auto")]
        [JsonPropertyName("check_auto")]
        public int CheckAuto { get; set; }
    }
}
