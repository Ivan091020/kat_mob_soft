using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Db
{
    [Table("reviews", Schema = "public")]
    public class ReviewDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long AppId { get; set; }
        public AppDb App { get; set; }

        public long? UserId { get; set; }
        public UserDb User { get; set; }

        [MaxLength(250)]
        public string Title { get; set; }

        public string Body { get; set; }

        [Range(1, 5)]
        public short Rating { get; set; }

        public bool IsApproved { get; set; } = false;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
