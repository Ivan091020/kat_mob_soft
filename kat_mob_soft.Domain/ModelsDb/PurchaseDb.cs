using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Db
{
    [System.ComponentModel.DataAnnotations.Schema.Table("purchases", Schema = "public")]
    public class PurchaseDb
    {
        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }
        public UserDb User { get; set; }

        public long AppId { get; set; }
        public AppDb App { get; set; }

        public long? VersionId { get; set; }
        public AppVersionDb Version { get; set; }

        public decimal PricePaid { get; set; }
        [MaxLength(3)]
        public string Currency { get; set; } = "USD";

        [MaxLength(100)]
        public string PaymentProvider { get; set; }

        [MaxLength(200)]
        public string ProviderTransactionId { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "completed";

        public DateTimeOffset PurchasedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
