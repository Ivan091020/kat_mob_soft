using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Db
{
    [Table("developers", Schema = "public")]
    public class DeveloperDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string Website { get; set; }

        [MaxLength(150)]
        public string ContactEmail { get; set; }

        public string Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<AppDb> Apps { get; set; }
    }
}
