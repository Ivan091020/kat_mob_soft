using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Db
{
    [Table("app_tags", Schema = "public")]
    public class AppTagDb
    {
        public long AppId { get; set; }
        public AppDb App { get; set; }

        public long TagId { get; set; }
        public TagDb Tag { get; set; }
    }
}
