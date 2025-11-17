using System;
using System.Collections.Generic;

namespace kat_mob_soft.Domain.Models
{
    public class Developer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string ContactEmail { get; set; }

        public ICollection<App> Apps { get; set; }
    }
}
