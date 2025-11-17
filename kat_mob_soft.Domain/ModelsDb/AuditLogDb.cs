using System;
using System.Text.Json;

namespace Domain.Models.Db
{
    [System.ComponentModel.DataAnnotations.Schema.Table("audit_logs", Schema = "public")]
    public class AuditLogDb
    {
        public long Id { get; set; }
        public string EntityType { get; set; }
        public long? EntityId { get; set; }
        public string Action { get; set; }
        public long? ActorUserId { get; set; }
        public UserDb Actor { get; set; }
        public JsonDocument Payload { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
