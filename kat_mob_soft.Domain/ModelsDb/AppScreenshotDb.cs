using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Db
{
    [Table("app_screenshots", Schema = "public")]
    public class AppScreenshotDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long AppId { get; set; }
        public AppDb App { get; set; }

        public long? VersionId { get; set; }
        public AppVersionDb Version { get; set; }

        [Required, MaxLength(1000)]
        public string FilePath { get; set; }

        public int SortOrder { get; set; } = 0;

        [MaxLength(300)]
        public string Caption { get; set; }
    }
}
