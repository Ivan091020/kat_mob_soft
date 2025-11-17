using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL;

namespace kat_mob_soft.DAL.Interfaces.Storage
{
    public class ReviewStorage : IBaseStorage<ReviewDb>
    {
        private readonly AppCatalogDbContext _db;
        public ReviewStorage(AppCatalogDbContext db) => _db = db;

        public async Task<IReadOnlyCollection<ReviewDb>> GetAllAsync()
        {
            var list = await _db.Reviews.Include(r => r.App).Include(r => r.User).ToListAsync();
            return list;
        }

        public async Task<ReviewDb> GetByIdAsync(Guid id)
        {
            var entity = await _db.Reviews.FindAsync(id);
            if (entity == null) return null;

            var entry = _db.Entry(entity);
            if (!entry.Reference(e => e.App).IsLoaded) await entry.Reference(e => e.App).LoadAsync();
            if (!entry.Reference(e => e.User).IsLoaded) await entry.Reference(e => e.User).LoadAsync();

            return entity;
        }

        public async Task CreateAsync(ReviewDb entity)
        {
            await _db.Reviews.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(ReviewDb entity)
        {
            _db.Reviews.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Reviews.FindAsync(id);
            if (entity != null)
            {
                _db.Reviews.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
