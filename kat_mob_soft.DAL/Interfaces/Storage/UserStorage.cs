using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL;

namespace kat_mob_soft.DAL.Interfaces.Storage
{
    public class UserStorage : IBaseStorage<UserDb>
    {
        private readonly AppCatalogDbContext _db;
        public UserStorage(AppCatalogDbContext db) => _db = db;

        public async Task<IReadOnlyCollection<UserDb>> GetAllAsync()
        {
            var list = await _db.Users.ToListAsync();
            return list;
        }

        public Task<UserDb> GetByIdAsync(Guid id)
        {
            // UserDb использует long, а не Guid
            return Task.FromResult<UserDb>(null);
        }

        public async Task<UserDb> GetByIdAsync(long id)
        {
            var entity = await _db.Users.FindAsync(id);
            if (entity == null) return null;

            // load navs if needed
            var entry = _db.Entry(entity);
            if (!entry.Collection(e => e.Reviews).IsLoaded) await entry.Collection(e => e.Reviews).LoadAsync();
            if (!entry.Collection(e => e.Downloads).IsLoaded) await entry.Collection(e => e.Downloads).LoadAsync();
            if (!entry.Collection(e => e.Purchases).IsLoaded) await entry.Collection(e => e.Purchases).LoadAsync();
            if (!entry.Collection(e => e.ReportsFiled).IsLoaded) await entry.Collection(e => e.ReportsFiled).LoadAsync();
            if (!entry.Collection(e => e.AuditLogs).IsLoaded) await entry.Collection(e => e.AuditLogs).LoadAsync();

            return entity;
        }

        public async Task<UserDb> GetByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task CreateAsync(UserDb entity)
        {
            Console.WriteLine($"UserStorage: Добавляем пользователя {entity.Email} в контекст...");
            await _db.Users.AddAsync(entity);
            Console.WriteLine("UserStorage: Вызываем SaveChangesAsync...");
            await _db.SaveChangesAsync();
            Console.WriteLine("UserStorage: SaveChangesAsync выполнен успешно");
        }

        public async Task UpdateAsync(UserDb entity)
        {
            _db.Users.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Users.FindAsync(id);
            if (entity != null)
            {
                _db.Users.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
