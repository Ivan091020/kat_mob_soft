using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL;

namespace kat_mob_soft.DAL.Interfaces.Storage
{
    public class AppStorage : IBaseStorage<AppDb>
    {
        private readonly AppCatalogDbContext _db;
        public AppStorage(AppCatalogDbContext db) => _db = db;

        // Возвращаем IReadOnlyCollection<T> — List<T> уже реализует IReadOnlyCollection<T>
        public async Task<IReadOnlyCollection<AppDb>> GetAllAsync()
        {
            var list = await _db.Apps
                                .Include(a => a.Category)
                                .Include(a => a.Developer)
                                .ToListAsync();
            return list;
        }

        // Чтобы не делать сравнение (long == Guid) — используем FindAsync
        public async Task<AppDb> GetByIdAsync(Guid id)
        {
            // FindAsync принимает object[] ключей — если тип PK в модели long/Guid несоответствует
            // сигнатуре Guid, компиляция пройдёт, а при выполнении ключ не найдётся — это безопаснее
            var entity = await _db.Apps.FindAsync(id);
            if (entity == null) return null;

            // Загрузим навигационные свойства вручную, если они ещё не загружены
            var entry = _db.Entry(entity);
            if (!entry.Reference(e => e.Category).IsLoaded) await entry.Reference(e => e.Category).LoadAsync();
            if (!entry.Reference(e => e.Developer).IsLoaded) await entry.Reference(e => e.Developer).LoadAsync();

            return entity;
        }

        public async Task CreateAsync(AppDb entity)
        {
            await _db.Apps.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppDb entity)
        {
            _db.Apps.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Apps.FindAsync(id);
            if (entity != null)
            {
                _db.Apps.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
