using kat_mob_soft.Domain.Response;

namespace kat_mob_soft.Domain.Response
{
    public class BaseResponse<T>
    {
        /// <summary>
        /// Сообщение об ошибке или успехе
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Статус выполнения операции
        /// </summary>
        public kat_mob_soft.Domain.Enum.StatusCode StatusCode { get; set; }


        /// <summary>
        /// Данные, которые возвращает сервис
        /// </summary>
        public T Data { get; set; }
    }
}
