using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kat_mob_soft.Domain.Models.Db
{
    [Table("app_versions", Schema = "public")]
    public class AppVersionDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long AppId { get; set; }
        public AppDb App { get; set; }

        [Required, MaxLength(50)]
        public string Version { get; set; }

        [Required, MaxLength(50)]
        public string Platform { get; set; }

        [MaxLength(200)]
        public string PackageId { get; set; }

        public string ReleaseNotes { get; set; }

        public long? FileSizeBytes { get; set; }

        [MaxLength(50)]
        public string MinOsVersion { get; set; }

        [MaxLength(1000)]
        public string DownloadUrl { get; set; }

        [MaxLength(200)]
        public string Checksum { get; set; }

        public DateTimeOffset UploadedAt { get; set; } = DateTimeOffset.UtcNow;
        public bool IsActive { get; set; } = true;

        public ICollection<AppScreenshotDb> Screenshots { get; set; }
        public ICollection<DownloadDb> Downloads { get; set; }
        public ICollection<PurchaseDb> Purchases { get; set; }
    }
}
