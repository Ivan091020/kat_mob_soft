using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL;

namespace kat_mob_soft.DAL.Interfaces.Storage
{
    public class AuditLogStorage : IBaseStorage<AuditLogDb>
    {
        private readonly AppCatalogDbContext _db;
        public AuditLogStorage(AppCatalogDbContext db) => _db = db;

        public async Task<IReadOnlyCollection<AuditLogDb>> GetAllAsync()
        {
            var list = await _db.AuditLogs.Include(a => a.Actor).ToListAsync();
            return list;
        }

        public async Task<AuditLogDb> GetByIdAsync(Guid id)
        {
            var entity = await _db.AuditLogs.FindAsync(id);
            if (entity == null) return null;

            var entry = _db.Entry(entity);
            if (!entry.Reference(e => e.Actor).IsLoaded) await entry.Reference(e => e.Actor).LoadAsync();

            return entity;
        }

        public async Task CreateAsync(AuditLogDb entity)
        {
            await _db.AuditLogs.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(AuditLogDb entity)
        {
            _db.AuditLogs.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.AuditLogs.FindAsync(id);
            if (entity != null)
            {
                _db.AuditLogs.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
