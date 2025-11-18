namespace kat_mob_soft.Domain.Enum
{
    public enum UserRole
    {
        User,
        Developer,
        Moderator,
        Admin
    }

    public enum AppPlatform
    {
        Android,
        iOS,
        Windows,
        Other
    }

    public enum PurchaseStatus
    {
        Pending,
        Completed,
        Refunded
    }
}