namespace kat_mob_soft.Domain.Enum
{
    public enum StatusCode
    {
        OK = 200,

        // Ошибки уровня пользователя
        UserNotFound = 404,
        UserAlreadyExists = 409,
        InvalidPassword = 401,

        // Ошибки уровня сущностей (каталог мобильных приложений)
        AppNotFound = 420,
        AppAlreadyExists = 421,

        // Общие ошибки
        InternalServerError = 500
    }
}
