using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL;

namespace kat_mob_soft.DAL.Interfaces.Storage
{
    public class CategoryStorage : IBaseStorage<CategoryDb>
    {
        private readonly AppCatalogDbContext _db;
        public CategoryStorage(AppCatalogDbContext db) => _db = db;

        public async Task<IReadOnlyCollection<CategoryDb>> GetAllAsync()
        {
            var list = await _db.Categories.ToListAsync();
            return list;
        }

        public async Task<CategoryDb> GetByIdAsync(Guid id)
        {
            var entity = await _db.Categories.FindAsync(id);
            return entity;
        }

        public async Task CreateAsync(CategoryDb entity)
        {
            await _db.Categories.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(CategoryDb entity)
        {
            _db.Categories.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Categories.FindAsync(id);
            if (entity != null)
            {
                _db.Categories.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
