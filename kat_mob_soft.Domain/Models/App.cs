using System;
using System.Collections.Generic;

namespace kat_mob_soft.Domain.Models
{
    public class App
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid? DeveloperId { get; set; }
        public Developer Developer { get; set; }
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ImagePath { get; set; }
        public double Rating { get; set; }

        public ICollection<AppVersion> Versions { get; set; }
        public ICollection<AppIcon> Icons { get; set; }
        public ICollection<AppScreenshot> Screenshots { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Download> Downloads { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
        public ICollection<AppTag> AppTags { get; set; }
    }
}
