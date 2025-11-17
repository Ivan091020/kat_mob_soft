using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Db
{
    [Table("app_icons", Schema = "public")]
    public class AppIconDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long AppId { get; set; }
        public AppDb App { get; set; }

        [Required, MaxLength(1000)]
        public string FilePath { get; set; }

        public DateTimeOffset UploadedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
