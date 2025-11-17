using System;

namespace kat_mob_soft.Domain.Models
{
    public class AppTag
    {
        public Guid AppId { get; set; }
        public App App { get; set; }

        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
