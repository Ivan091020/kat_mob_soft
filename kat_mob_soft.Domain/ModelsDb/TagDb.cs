using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Db
{
    [Table("tags", Schema = "public")]
    public class TagDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public ICollection<AppTagDb> AppTags { get; set; }
    }
}
