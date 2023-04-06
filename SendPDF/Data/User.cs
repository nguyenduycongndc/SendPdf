using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SendMailPDF.Data
{
    [Table("users")]
    public class Users
    {
        public Users()
        {
            UsersRoles = new HashSet<UsersRoles>();
        }
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Column("full_name")]
        [JsonPropertyName("full_name")]
        public string? FullName { get; set; }

        [Column("user_name")]
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }

        [Column("date_of_joining")]
        [JsonPropertyName("date_of_joining")]
        public DateTime? DateOfJoining { get; set; }

        [Column("role_id")]
        [JsonPropertyName("role_id")]
        public int? RoleId { get; set; }

        [Column("is_active")]
        [JsonPropertyName("is_active")]
        public int IsActive { get; set; }

        [Column("is_deleted")]
        [JsonPropertyName("is_deleted")]
        public int? IsDeleted { get; set; }

        [Column("email")]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("email_password")]
        [Column(TypeName = "nvarchar")]
        [StringLength(30)]
        public string? EmailPassword { get; set; }

        [Column("password")]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [Column("salt")]
        [JsonPropertyName("salt")]
        public string? SaltKey { get; set; }

        [Column("created_by")]
        [JsonPropertyName("created_by")]
        public int? CreatedBy { get; set; }

        [Column("created_at")]
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("modified_by")]
        [JsonPropertyName("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("modified_at")]
        [JsonPropertyName("modified_at")]
        public DateTime? ModifiedAt { get; set; }

        [Column("deleted_at")]
        [JsonPropertyName("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        [Column("deleted_by")]
        [JsonPropertyName("deleted_by")]
        public int? DeletedBy { get; set; }

        [Column("expiration_date_otp")]
        [JsonPropertyName("expiration_date_otp")]
        public DateTime? ExpirationDateOtp { get; set; }

        [Column("otp")]
        [JsonPropertyName("otp")]
        public int? OTP { get; set; }
        public virtual ICollection<UsersRoles> UsersRoles { get; set; }
    }
}
