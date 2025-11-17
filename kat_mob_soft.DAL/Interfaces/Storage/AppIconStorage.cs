using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL;

namespace kat_mob_soft.DAL.Interfaces.Storage
{
    public class AppIconStorage : IBaseStorage<AppIconDb>
    {
        private readonly AppCatalogDbContext _db;
        public AppIconStorage(AppCatalogDbContext db) => _db = db;

        public async Task<IReadOnlyCollection<AppIconDb>> GetAllAsync()
        {
            var list = await _db.AppIcons.Include(i => i.App).ToListAsync();
            return list;
        }

        public async Task<AppIconDb> GetByIdAsync(Guid id)
        {
            var entity = await _db.AppIcons.FindAsync(id);
            if (entity == null) return null;

            var entry = _db.Entry(entity);
            if (!entry.Reference(e => e.App).IsLoaded) await entry.Reference(e => e.App).LoadAsync();

            return entity;
        }

        public async Task CreateAsync(AppIconDb entity)
        {
            await _db.AppIcons.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppIconDb entity)
        {
            _db.AppIcons.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.AppIcons.FindAsync(id);
            if (entity != null)
            {
                _db.AppIcons.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
