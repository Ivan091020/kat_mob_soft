using System;

namespace kat_mob_soft.Domain.Models
{
    public class AppVersion
    {
        public Guid Id { get; set; }
        public Guid AppId { get; set; }
        public App App { get; set; }
        public string VersionNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
