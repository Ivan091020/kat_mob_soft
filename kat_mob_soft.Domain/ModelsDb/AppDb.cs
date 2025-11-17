using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Domain.Models.Db
{
    [Table("apps", Schema = "public")]
    public class AppDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long? DeveloperId { get; set; }
        public DeveloperDb Developer { get; set; }

        public long? CategoryId { get; set; }
        public CategoryDb Category { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required, MaxLength(200)]
        public string Slug { get; set; }

        [MaxLength(500)]
        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public decimal Price { get; set; } = 0m;

        [MaxLength(3)]
        public string Currency { get; set; } = "USD";

        public bool IsPublished { get; set; } = false;
        public DateTimeOffset? PublishedAt { get; set; }
        public decimal AverageRating { get; set; } = 0m;
        public int TotalReviews { get; set; } = 0;

        // jsonb
        public JsonDocument Metadata { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<AppVersionDb> Versions { get; set; }
        public ICollection<AppScreenshotDb> Screenshots { get; set; }
        public AppIconDb Icon { get; set; }
        public ICollection<AppTagDb> AppTags { get; set; }
        public ICollection<ReviewDb> Reviews { get; set; }
        public ICollection<DownloadDb> Downloads { get; set; }
        public ICollection<PurchaseDb> Purchases { get; set; }
    }
}
