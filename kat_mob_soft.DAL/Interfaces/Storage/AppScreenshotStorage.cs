using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL;

namespace kat_mob_soft.DAL.Interfaces.Storage
{
    public class AppScreenshotStorage : IBaseStorage<AppScreenshotDb>
    {
        private readonly AppCatalogDbContext _db;
        public AppScreenshotStorage(AppCatalogDbContext db) => _db = db;

        public async Task<IReadOnlyCollection<AppScreenshotDb>> GetAllAsync()
        {
            var list = await _db.AppScreenshots.Include(s => s.App).ToListAsync();
            return list;
        }

        public async Task<AppScreenshotDb> GetByIdAsync(Guid id)
        {
            var entity = await _db.AppScreenshots.FindAsync(id);
            if (entity == null) return null;

            var entry = _db.Entry(entity);
            if (!entry.Reference(e => e.App).IsLoaded) await entry.Reference(e => e.App).LoadAsync();

            return entity;
        }

        public async Task CreateAsync(AppScreenshotDb entity)
        {
            await _db.AppScreenshots.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppScreenshotDb entity)
        {
            _db.AppScreenshots.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.AppScreenshots.FindAsync(id);
            if (entity != null)
            {
                _db.AppScreenshots.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
