using System;

namespace kat_mob_soft.Domain.Models
{
    public class AppScreenshot
    {
        public Guid Id { get; set; }
        public Guid AppId { get; set; }
        public App App { get; set; }
        public string Path { get; set; }
    }
}
