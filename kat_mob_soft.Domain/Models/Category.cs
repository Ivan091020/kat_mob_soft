using System;
using System.Collections.Generic;

namespace kat_mob_soft.Domain.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }

        public ICollection<App> Apps { get; set; }
    }
}
