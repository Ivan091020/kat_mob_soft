using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL;

namespace kat_mob_soft.DAL.Interfaces.Storage
{
    public class DownloadStorage : IBaseStorage<DownloadDb>
    {
        private readonly AppCatalogDbContext _db;
        public DownloadStorage(AppCatalogDbContext db) => _db = db;

        public async Task<IReadOnlyCollection<DownloadDb>> GetAllAsync()
        {
            var list = await _db.Downloads.Include(d => d.App).Include(d => d.User).ToListAsync();
            return list;
        }

        public async Task<DownloadDb> GetByIdAsync(Guid id)
        {
            var entity = await _db.Downloads.FindAsync(id);
            if (entity == null) return null;

            var entry = _db.Entry(entity);
            if (!entry.Reference(e => e.App).IsLoaded) await entry.Reference(e => e.App).LoadAsync();
            if (!entry.Reference(e => e.User).IsLoaded) await entry.Reference(e => e.User).LoadAsync();

            return entity;
        }

        public async Task CreateAsync(DownloadDb entity)
        {
            await _db.Downloads.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(DownloadDb entity)
        {
            _db.Downloads.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Downloads.FindAsync(id);
            if (entity != null)
            {
                _db.Downloads.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
