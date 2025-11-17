using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kat_mob_soft.Domain.Models.Db
{
    [Table("users", Schema = "public")]
    public class UserDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; }

        [Required, MaxLength(150)]
        public string Email { get; set; }

        [Required, MaxLength(255)]
        public string PasswordHash { get; set; }

        [MaxLength(200)]
        public string DisplayName { get; set; }

        [MaxLength(50)]
        public string Role { get; set; } = "user";

        [MaxLength(1000)]
        public string AvatarPath { get; set; }

        public DateTimeOffset RegisteredAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? LastLogin { get; set; }

        // --- Навигационные свойства (добавлено) ---
        public virtual ICollection<ReviewDb> Reviews { get; set; } = new List<ReviewDb>();
        public virtual ICollection<DownloadDb> Downloads { get; set; } = new List<DownloadDb>();
        public virtual ICollection<PurchaseDb> Purchases { get; set; } = new List<PurchaseDb>();
        public virtual ICollection<ReportDb> ReportsFiled { get; set; } = new List<ReportDb>();
        public virtual ICollection<AuditLogDb> AuditLogs { get; set; } = new List<AuditLogDb>();

    }
}
