// DAL/Data/AppCatalogDbContext.cs
using System;
using kat_mob_soft.Domain.Models.Db;
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
            // Явное указание имен столбцов для PostgreSQL (snake_case)
            modelBuilder.Entity<UserDb>().Property(u => u.Id).HasColumnName("id");
            modelBuilder.Entity<UserDb>().Property(u => u.Username).HasColumnName("username");
            modelBuilder.Entity<UserDb>().Property(u => u.Email).HasColumnName("email");
            modelBuilder.Entity<UserDb>().Property(u => u.PasswordHash).HasColumnName("password_hash");
            modelBuilder.Entity<UserDb>().Property(u => u.DisplayName).HasColumnName("display_name");
            modelBuilder.Entity<UserDb>().Property(u => u.Role).HasColumnName("role");
            modelBuilder.Entity<UserDb>().Property(u => u.AvatarPath).HasColumnName("avatar_path");
            modelBuilder.Entity<UserDb>().Property(u => u.RegisteredAt).HasColumnName("registered_at");
            modelBuilder.Entity<UserDb>().Property(u => u.LastLogin).HasColumnName("last_login");
            modelBuilder.Entity<UserDb>().Property(u => u.EmailConfirmed).HasColumnName("email_confirmed");
            modelBuilder.Entity<UserDb>().Property(u => u.EmailConfirmationToken).HasColumnName("email_confirmation_token");
            modelBuilder.Entity<DeveloperDb>().ToTable("developers", "public");
            // Настройка имен столбцов для DeveloperDb (snake_case)
            modelBuilder.Entity<DeveloperDb>().Property(d => d.Id).HasColumnName("id");
            modelBuilder.Entity<DeveloperDb>().Property(d => d.Name).HasColumnName("name");
            modelBuilder.Entity<DeveloperDb>().Property(d => d.Website).HasColumnName("website");
            modelBuilder.Entity<DeveloperDb>().Property(d => d.ContactEmail).HasColumnName("contact_email");
            modelBuilder.Entity<DeveloperDb>().Property(d => d.Description).HasColumnName("description");
            modelBuilder.Entity<DeveloperDb>().Property(d => d.CreatedAt).HasColumnName("created_at");

            modelBuilder.Entity<CategoryDb>().ToTable("categories", "public");
            // Настройка имен столбцов для CategoryDb (snake_case)
            modelBuilder.Entity<CategoryDb>().Property(c => c.Id).HasColumnName("id");
            modelBuilder.Entity<CategoryDb>().Property(c => c.Name).HasColumnName("name");
            modelBuilder.Entity<CategoryDb>().Property(c => c.Slug).HasColumnName("slug");
            modelBuilder.Entity<CategoryDb>().Property(c => c.ParentId).HasColumnName("parent_id");
            modelBuilder.Entity<CategoryDb>().Property(c => c.Description).HasColumnName("description");
            modelBuilder.Entity<CategoryDb>().Property(c => c.CreatedAt).HasColumnName("created_at");

            modelBuilder.Entity<TagDb>().ToTable("tags", "public");
            // Настройка имен столбцов для TagDb (snake_case)
            modelBuilder.Entity<TagDb>().Property(t => t.Id).HasColumnName("id");
            modelBuilder.Entity<TagDb>().Property(t => t.Name).HasColumnName("name");

            modelBuilder.Entity<AppDb>().ToTable("apps", "public");
            // Настройка имен столбцов для AppDb (snake_case)
            modelBuilder.Entity<AppDb>().Property(a => a.Id).HasColumnName("id");
            modelBuilder.Entity<AppDb>().Property(a => a.Name).HasColumnName("name");
            modelBuilder.Entity<AppDb>().Property(a => a.Slug).HasColumnName("slug");
            modelBuilder.Entity<AppDb>().Property(a => a.ShortDescription).HasColumnName("short_description");
            modelBuilder.Entity<AppDb>().Property(a => a.FullDescription).HasColumnName("full_description");
            modelBuilder.Entity<AppDb>().Property(a => a.Price).HasColumnName("price");
            modelBuilder.Entity<AppDb>().Property(a => a.Currency).HasColumnName("currency");
            modelBuilder.Entity<AppDb>().Property(a => a.IsPublished).HasColumnName("is_published");
            modelBuilder.Entity<AppDb>().Property(a => a.PublishedAt).HasColumnName("published_at");
            modelBuilder.Entity<AppDb>().Property(a => a.AverageRating).HasColumnName("average_rating");
            modelBuilder.Entity<AppDb>().Property(a => a.TotalReviews).HasColumnName("total_reviews");
            modelBuilder.Entity<AppDb>().Property(a => a.CreatedAt).HasColumnName("created_at");
            modelBuilder.Entity<AppDb>().Property(a => a.UpdatedAt).HasColumnName("updated_at");
            modelBuilder.Entity<AppDb>().Property(a => a.DeveloperId).HasColumnName("developer_id");
            modelBuilder.Entity<AppDb>().Property(a => a.CategoryId).HasColumnName("category_id");
            modelBuilder.Entity<AppDb>().Property(a => a.Metadata).HasColumnName("metadata");
            modelBuilder.Entity<AppVersionDb>().ToTable("app_versions", "public");
            modelBuilder.Entity<AppScreenshotDb>().ToTable("app_screenshots", "public");
            // Настройка имен столбцов для AppScreenshotDb (snake_case)
            modelBuilder.Entity<AppScreenshotDb>().Property(s => s.Id).HasColumnName("id");
            modelBuilder.Entity<AppScreenshotDb>().Property(s => s.AppId).HasColumnName("app_id");
            modelBuilder.Entity<AppScreenshotDb>().Property(s => s.VersionId).HasColumnName("version_id");
            modelBuilder.Entity<AppScreenshotDb>().Property(s => s.FilePath).HasColumnName("file_path");
            modelBuilder.Entity<AppScreenshotDb>().Property(s => s.SortOrder).HasColumnName("sort_order");
            modelBuilder.Entity<AppScreenshotDb>().Property(s => s.Caption).HasColumnName("caption");
            
            modelBuilder.Entity<AppIconDb>().ToTable("app_icons", "public");
            // Настройка имен столбцов для AppIconDb (snake_case)
            modelBuilder.Entity<AppIconDb>().Property(i => i.Id).HasColumnName("id");
            modelBuilder.Entity<AppIconDb>().Property(i => i.AppId).HasColumnName("app_id");
            modelBuilder.Entity<AppIconDb>().Property(i => i.FilePath).HasColumnName("file_path");
            modelBuilder.Entity<AppIconDb>().Property(i => i.UploadedAt).HasColumnName("uploaded_at");

            modelBuilder.Entity<AppTagDb>().ToTable("app_tags", "public");

            modelBuilder.Entity<ReviewDb>().ToTable("reviews", "public");
            modelBuilder.Entity<DownloadDb>().ToTable("downloads", "public");
            // Настройка имен столбцов для DownloadDb (snake_case)
            modelBuilder.Entity<DownloadDb>().Property(d => d.Id).HasColumnName("id");
            modelBuilder.Entity<DownloadDb>().Property(d => d.AppId).HasColumnName("app_id");
            modelBuilder.Entity<DownloadDb>().Property(d => d.VersionId).HasColumnName("version_id");
            modelBuilder.Entity<DownloadDb>().Property(d => d.UserId).HasColumnName("user_id");
            modelBuilder.Entity<DownloadDb>().Property(d => d.IpAddress).HasColumnName("ip_address");
            modelBuilder.Entity<DownloadDb>().Property(d => d.Platform).HasColumnName("platform");
            modelBuilder.Entity<DownloadDb>().Property(d => d.CreatedAt).HasColumnName("created_at");

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
                modelBuilder.Entity<AppDb>().Property(a => a.Metadata).HasColumnName("metadata").HasColumnType("jsonb");
                modelBuilder.Entity<AuditLogDb>().Property(nameof(AuditLogDb.Payload)).HasColumnType("jsonb");
            }

            // Доп. правила/ограничения можно добавить здесь (CHECK, DEFAULT и т.д.)
        }
    }
}
