using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Db
{
    [Table("downloads", Schema = "public")]
    public class DownloadDb
    {
        public long Id { get; set; }
        public long AppId { get; set; }
        public AppDb App { get; set; }

        public long? VersionId { get; set; }
        public AppVersionDb Version { get; set; }

        public long? UserId { get; set; }
        public UserDb User { get; set; }

        public string IpAddress { get; set; }
        public string Platform { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
