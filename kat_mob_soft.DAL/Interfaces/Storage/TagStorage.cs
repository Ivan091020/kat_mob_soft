using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL;

namespace kat_mob_soft.DAL.Interfaces.Storage
{
    public class TagStorage : IBaseStorage<TagDb>
    {
        private readonly AppCatalogDbContext _db;
        public TagStorage(AppCatalogDbContext db) => _db = db;

        public async Task<IReadOnlyCollection<TagDb>> GetAllAsync()
        {
            var list = await _db.Tags.ToListAsync();
            return list;
        }

        public async Task<TagDb> GetByIdAsync(Guid id)
        {
            var entity = await _db.Tags.FindAsync(id);
            return entity;
        }

        public async Task CreateAsync(TagDb entity)
        {
            await _db.Tags.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(TagDb entity)
        {
            _db.Tags.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Tags.FindAsync(id);
            if (entity != null)
            {
                _db.Tags.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
