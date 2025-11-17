using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kat_mob_soft.Domain.Models.Db
{
    [Table("categories", Schema = "public")]
    public class CategoryDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        [Required, MaxLength(150)]
        public string Slug { get; set; }

        public long? ParentId { get; set; }
        public CategoryDb Parent { get; set; }
        public ICollection<CategoryDb> Children { get; set; }

        public string Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<AppDb> Apps { get; set; }
    }
}
