// DAL/Data/AppCatalogDbContext.cs
using System;
using Domain.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace kat_mob_soft.DAL
{
    /// <summary>
    /// AppCatalogDbContext — основной DbContext для каталога мобильных приложений.
    /// Соответствует рисунку 117: все DbSet, конфигурация отношений, индексов и JSONB.
    /// Использовать в проекте DAL (Class Library).
    /// </summary>
    public class AppCatalogDbContext : DbContext
    {
        public AppCatalogDbContext(DbContextOptions<AppCatalogDbContext> options)
            : base(options) { }

        // DbSets — соответствуют существующим таблицам в kat_mob_soft
        public DbSet<UserDb> Users { get; set; }
        public DbSet<DeveloperDb> Developers { get; set; }
        public DbSet<CategoryDb> Categories { get; set; }
        public DbSet<TagDb> Tags { get; set; }

        public DbSet<AppDb> Apps { get; set; }
        public DbSet<AppVersionDb> AppVersions { get; set; }
        public DbSet<AppScreenshotDb> AppScreenshots { get; set; }
        public DbSet<AppIconDb> AppIcons { get; set; }
        public DbSet<AppTagDb> AppTags { get; set; }

        public DbSet<ReviewDb> Reviews { get; set; }
        public DbSet<DownloadDb> Downloads { get; set; }
        public DbSet<PurchaseDb> Purchases { get; set; }

        public DbSet<ReportDb> Reports { get; set; }
        public DbSet<AuditLogDb> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ----------------------------------------
            // Таблицы / имена — явная привязка (на случай, если в Domain нет [Table])
            // ----------------------------------------
            modelBuilder.Entity<UserDb>().ToTable("users", "public");
            modelBuilder.Entity<DeveloperDb>().ToTable("developers", "public");
            modelBuilder.Entity<CategoryDb>().ToTable("categories", "public");
            modelBuilder.Entity<TagDb>().ToTable("tags", "public");

            modelBuilder.Entity<AppDb>().ToTable("apps", "public");
            modelBuilder.Entity<AppVersionDb>().ToTable("app_versions", "public");
            modelBuilder.Entity<AppScreenshotDb>().ToTable("app_screenshots", "public");
            modelBuilder.Entity<AppIconDb>().ToTable("app_icons", "public");
            modelBuilder.Entity<AppTagDb>().ToTable("app_tags", "public");

            modelBuilder.Entity<ReviewDb>().ToTable("reviews", "public");
            modelBuilder.Entity<DownloadDb>().ToTable("downloads", "public");
            modelBuilder.Entity<PurchaseDb>().ToTable("purchases", "public");

            modelBuilder.Entity<ReportDb>().ToTable("reports", "public");
            modelBuilder.Entity<AuditLogDb>().ToTable("audit_logs", "public");

            // ----------------------------------------
            // Composite key: app_tags (many-to-many)
            // ----------------------------------------
            modelBuilder.Entity<AppTagDb>()
                .HasKey(at => new { at.AppId, at.TagId });

            // ----------------------------------------
            // Relations & Delete behaviors (как в SQL-скрипте)
            // ----------------------------------------
            modelBuilder.Entity<AppDb>()
                .HasOne(a => a.Developer)
                .WithMany(d => d.Apps)
                .HasForeignKey(a => a.DeveloperId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<AppDb>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Apps)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<AppVersionDb>()
                .HasOne(v => v.App)
                .WithMany(a => a.Versions)
                .HasForeignKey(v => v.AppId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppScreenshotDb>()
                .HasOne(s => s.App)
                .WithMany(a => a.Screenshots)
                .HasForeignKey(s => s.AppId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppIconDb>()
                .HasOne(i => i.App)
                .WithOne(a => a.Icon)
                .HasForeignKey<AppIconDb>(i => i.AppId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewDb>()
                .HasOne(r => r.App)
                .WithMany(a => a.Reviews)
                .HasForeignKey(r => r.AppId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewDb>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<DownloadDb>()
                .HasOne(d => d.App)
                .WithMany(a => a.Downloads)
                .HasForeignKey(d => d.AppId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DownloadDb>()
                .HasOne(d => d.User)
                .WithMany(u => u.Downloads)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PurchaseDb>()
                .HasOne(p => p.User)
                .WithMany(u => u.Purchases)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseDb>()
                .HasOne(p => p.App)
                .WithMany(a => a.Purchases)
                .HasForeignKey(p => p.AppId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReportDb>()
                .HasOne(r => r.Reporter)
                .WithMany(u => u.ReportsFiled)
                .HasForeignKey(r => r.ReporterUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<AuditLogDb>()
                .HasOne(a => a.Actor)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.ActorUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // ----------------------------------------
            // Уникальные ограничения и индексы (как в SQL)
            // ----------------------------------------
            modelBuilder.Entity<CategoryDb>()
                .HasIndex(c => c.Slug)
                .IsUnique();

            modelBuilder.Entity<AppDb>()
                .HasIndex(a => a.Slug)
                .IsUnique();

            modelBuilder.Entity<TagDb>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<UserDb>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<AppDb>()
                .HasIndex(a => a.DeveloperId)
                .HasDatabaseName("idx_apps_developer");

            modelBuilder.Entity<AppDb>()
                .HasIndex(a => a.CategoryId)
                .HasDatabaseName("idx_apps_category");

            modelBuilder.Entity<AppVersionDb>()
                .HasIndex(v => v.AppId)
                .HasDatabaseName("idx_app_versions_app");

            modelBuilder.Entity<AppScreenshotDb>()
                .HasIndex(s => s.AppId)
                .HasDatabaseName("idx_screenshots_app");

            modelBuilder.Entity<ReviewDb>()
                .HasIndex(r => r.AppId)
                .HasDatabaseName("idx_reviews_app");

            modelBuilder.Entity<DownloadDb>()
                .HasIndex(d => d.AppId)
                .HasDatabaseName("idx_downloads_app");

            modelBuilder.Entity<PurchaseDb>()
                .HasIndex(p => p.UserId)
                .HasDatabaseName("idx_purchases_user");

            // ----------------------------------------
            // JSON mapping: если в AppDb есть поле Metadata -> jsonb
            // (Npgsql автоматически мапит System.Text.Json.JsonDocument)
            // ----------------------------------------
            if (Database.ProviderName != null && Database.ProviderName.Contains("Npgsql"))
            {
                modelBuilder.Entity<AppDb>().Property(nameof(AppDb.Metadata)).HasColumnType("jsonb");
                modelBuilder.Entity<AuditLogDb>().Property(nameof(AuditLogDb.Payload)).HasColumnType("jsonb");
            }

            // Доп. правила/ограничения можно добавить здесь (CHECK, DEFAULT и т.д.)
        }
    }
}
