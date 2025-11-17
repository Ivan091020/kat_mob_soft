using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kat_mob_soft.DAL.Interfaces
{
    // Несмотря на имя файла IBaseStorageT (без <T> в названии файла),
    // сам интерфейс использует generic T — это нормально и так принято.
    public interface IBaseStorage<T> where T : class
    {
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
