using System;

namespace kat_mob_soft.Domain.Models
{
    public class Download
    {
        public Guid Id { get; set; }
        public Guid AppId { get; set; }
        public App App { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime DownloadedAt { get; set; }
    }
}
