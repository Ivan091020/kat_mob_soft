using System;
using System.Collections.Generic;

namespace kat_mob_soft.Domain.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<AppTag> AppTags { get; set; }
    }
}
