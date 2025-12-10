namespace kat_mob_soft.Domain.Enum
{
    public enum StatusCode
    {
        OK = 200,

        // Общие HTTP ошибки
        BadRequest = 400,
        InvalidPassword = 401,
        Forbidden = 403,
        NotFound = 404,
        UserAlreadyExists = 409,

        // Ошибки уровня пользователя
        UserNotFound = 410,

        // Ошибки уровня сущностей (каталог мобильных приложений)
        AppNotFound = 420,
        AppAlreadyExists = 421,

        // Общие ошибки
        InternalServerError = 500
    }
}
