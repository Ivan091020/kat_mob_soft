using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL;

namespace kat_mob_soft.DAL.Interfaces.Storage
{
    public class DeveloperStorage : IBaseStorage<DeveloperDb>
    {
        private readonly AppCatalogDbContext _db;
        public DeveloperStorage(AppCatalogDbContext db) => _db = db;

        public async Task<IReadOnlyCollection<DeveloperDb>> GetAllAsync()
        {
            var list = await _db.Developers.ToListAsync();
            return list;
        }

        public async Task<DeveloperDb> GetByIdAsync(Guid id)
        {
            var entity = await _db.Developers.FindAsync(id);
            return entity;
        }

        public async Task CreateAsync(DeveloperDb entity)
        {
            await _db.Developers.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(DeveloperDb entity)
        {
            _db.Developers.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Developers.FindAsync(id);
            if (entity != null)
            {
                _db.Developers.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
