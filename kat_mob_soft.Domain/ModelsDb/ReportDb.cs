using System;

namespace kat_mob_soft.Domain.Models.Db
{
    [System.ComponentModel.DataAnnotations.Schema.Table("reports", Schema = "public")]
    public class ReportDb
    {
        public long Id { get; set; }

        public long? ReporterUserId { get; set; }
        public UserDb Reporter { get; set; }

        public long? AppId { get; set; }
        public AppDb App { get; set; }

        public long? ReviewId { get; set; }
        public ReviewDb Review { get; set; }

        public string Reason { get; set; }
        public string Details { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public bool Resolved { get; set; } = false;
        public DateTimeOffset? ResolvedAt { get; set; }
    }
}
