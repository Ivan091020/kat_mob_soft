using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL;

namespace kat_mob_soft.DAL.Interfaces.Storage
{
    public class AppTagStorage : IBaseStorage<AppTagDb>
    {
        private readonly AppCatalogDbContext _db;
        public AppTagStorage(AppCatalogDbContext db) => _db = db;

        public async Task<IReadOnlyCollection<AppTagDb>> GetAllAsync()
        {
            var list = await _db.AppTags
                                .Include(at => at.App)
                                .Include(at => at.Tag)
                                .ToListAsync();
            return list;
        }

        // Интерфейс требует GetByIdAsync(Guid) — но у AppTag композитный ключ.
        // Реализуем интерфейсный метод заглушкой и добавим реальные методы для composite key.
        public Task<AppTagDb> GetByIdAsync(Guid id) =>
            throw new NotImplementedException("AppTag uses composite key (appId, tagId). Use GetByIdsAsync(appId, tagId).");

        public Task DeleteAsync(Guid id) =>
            throw new NotImplementedException("AppTag uses composite key (appId, tagId). Use DeleteAsync(appId, tagId).");

        // Реальные методы для composite key:
        public async Task<AppTagDb> GetByIdsAsync(Guid appId, Guid tagId)
        {
            var entity = await _db.AppTags.FindAsync(appId, tagId);
            if (entity == null) return null;

            var entry = _db.Entry(entity);
            if (!entry.Reference(e => e.App).IsLoaded) await entry.Reference(e => e.App).LoadAsync();
            if (!entry.Reference(e => e.Tag).IsLoaded) await entry.Reference(e => e.Tag).LoadAsync();

            return entity;
        }

        public async Task CreateAsync(AppTagDb entity)
        {
            await _db.AppTags.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppTagDb entity)
        {
            _db.AppTags.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid appId, Guid tagId)
        {
            var entity = await _db.AppTags.FindAsync(appId, tagId);
            if (entity != null)
            {
                _db.AppTags.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
