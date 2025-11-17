using System;

namespace kat_mob_soft.Domain.Models
{
    public class Report
    {
        public Guid Id { get; set; }
        public Guid ReporterUserId { get; set; }
        public User Reporter { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
