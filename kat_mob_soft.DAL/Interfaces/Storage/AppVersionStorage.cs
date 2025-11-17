using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL;

namespace kat_mob_soft.DAL.Interfaces.Storage
{
    public class AppVersionStorage : IBaseStorage<AppVersionDb>
    {
        private readonly AppCatalogDbContext _db;
        public AppVersionStorage(AppCatalogDbContext db) => _db = db;

        public async Task<IReadOnlyCollection<AppVersionDb>> GetAllAsync()
        {
            var list = await _db.AppVersions.Include(v => v.App).ToListAsync();
            return list;
        }

        public async Task<AppVersionDb> GetByIdAsync(Guid id)
        {
            var entity = await _db.AppVersions.FindAsync(id);
            if (entity == null) return null;

            var entry = _db.Entry(entity);
            if (!entry.Reference(e => e.App).IsLoaded) await entry.Reference(e => e.App).LoadAsync();

            return entity;
        }

        public async Task CreateAsync(AppVersionDb entity)
        {
            await _db.AppVersions.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppVersionDb entity)
        {
            _db.AppVersions.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.AppVersions.FindAsync(id);
            if (entity != null)
            {
                _db.AppVersions.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
