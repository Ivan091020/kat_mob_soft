using System;
using System.Collections.Generic;

namespace kat_mob_soft.Domain.Filter
{
    public class AppFilter
    {
        public decimal PriceMin { get; set; }
        public decimal PriceMax { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
    }
}
