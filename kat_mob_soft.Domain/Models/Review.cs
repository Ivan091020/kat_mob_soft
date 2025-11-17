using System;

namespace kat_mob_soft.Domain.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid AppId { get; set; }
        public App App { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
