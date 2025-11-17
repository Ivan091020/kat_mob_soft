using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL;

namespace kat_mob_soft.DAL.Interfaces.Storage
{
    public class PurchaseStorage : IBaseStorage<PurchaseDb>
    {
        private readonly AppCatalogDbContext _db;
        public PurchaseStorage(AppCatalogDbContext db) => _db = db;

        public async Task<IReadOnlyCollection<PurchaseDb>> GetAllAsync()
        {
            var list = await _db.Purchases.Include(p => p.App).Include(p => p.User).ToListAsync();
            return list;
        }

        public async Task<PurchaseDb> GetByIdAsync(Guid id)
        {
            var entity = await _db.Purchases.FindAsync(id);
            if (entity == null) return null;

            var entry = _db.Entry(entity);
            if (!entry.Reference(e => e.App).IsLoaded) await entry.Reference(e => e.App).LoadAsync();
            if (!entry.Reference(e => e.User).IsLoaded) await entry.Reference(e => e.User).LoadAsync();

            return entity;
        }

        public async Task CreateAsync(PurchaseDb entity)
        {
            await _db.Purchases.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(PurchaseDb entity)
        {
            _db.Purchases.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Purchases.FindAsync(id);
            if (entity != null)
            {
                _db.Purchases.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
