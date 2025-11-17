using System;
using System.Collections.Generic;

namespace kat_mob_soft.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public ICollection<Download> Downloads { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
        public ICollection<Report> ReportsFiled { get; set; }
        public ICollection<AuditLog> AuditLogs { get; set; }
    }
}
