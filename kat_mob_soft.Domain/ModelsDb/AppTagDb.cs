using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kat_mob_soft.Domain.Models.Db
{
    [Table("app_tags", Schema = "public")]
    public class AppTagDb
    {
        [Key]
        [Column(Order = 0)]
        public long AppId { get; set; }
        
        [ForeignKey("AppId")]
        public AppDb App { get; set; }

        [Key]
        [Column(Order = 1)]
        public long TagId { get; set; }
        
        [ForeignKey("TagId")]
        public TagDb Tag { get; set; }
    }
}
