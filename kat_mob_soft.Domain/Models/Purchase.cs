using System;

namespace kat_mob_soft.Domain.Models
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public Guid AppId { get; set; }
        public App App { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime PurchasedAt { get; set; }
        public decimal Price { get; set; }
    }
}
