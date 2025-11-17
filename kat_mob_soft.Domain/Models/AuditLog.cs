using System;

namespace kat_mob_soft.Domain.Models
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public Guid ActorUserId { get; set; }
        public User Actor { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string Payload { get; set; } // JSON
    }
}
