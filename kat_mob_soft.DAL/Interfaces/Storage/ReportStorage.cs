using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL;

namespace kat_mob_soft.DAL.Interfaces.Storage
{
    public class ReportStorage : IBaseStorage<ReportDb>
    {
        private readonly AppCatalogDbContext _db;
        public ReportStorage(AppCatalogDbContext db) => _db = db;

        public async Task<IReadOnlyCollection<ReportDb>> GetAllAsync()
        {
            var list = await _db.Reports.Include(r => r.Reporter).ToListAsync();
            return list;
        }

        public async Task<ReportDb> GetByIdAsync(Guid id)
        {
            var entity = await _db.Reports.FindAsync(id);
            if (entity == null) return null;

            var entry = _db.Entry(entity);
            if (!entry.Reference(e => e.Reporter).IsLoaded) await entry.Reference(e => e.Reporter).LoadAsync();

            return entity;
        }

        public async Task CreateAsync(ReportDb entity)
        {
            await _db.Reports.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(ReportDb entity)
        {
            _db.Reports.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Reports.FindAsync(id);
            if (entity != null)
            {
                _db.Reports.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
