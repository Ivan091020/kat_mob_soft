using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace kat_mob_soft.DAL.Migrations.Init
{
    public partial class INIT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        //    migrationBuilder.EnsureSchema(
        //        name: "public");

        //    migrationBuilder.CreateTable(
        //        name: "categories",
        //        schema: "public",
        //        columns: table => new
        //        {
        //            Id = table.Column<long>(type: "bigint", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
        //            Slug = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
        //            ParentId = table.Column<long>(type: "bigint", nullable: true),
        //            Description = table.Column<string>(type: "text", nullable: true),
        //            CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_categories", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_categories_categories_ParentId",
        //                column: x => x.ParentId,
        //                principalSchema: "public",
        //                principalTable: "categories",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "developers",
        //        schema: "public",
        //        columns: table => new
        //        {
        //            Id = table.Column<long>(type: "bigint", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
        //            Website = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
        //            ContactEmail = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
        //            Description = table.Column<string>(type: "text", nullable: true),
        //            CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_developers", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "tags",
        //        schema: "public",
        //        columns: table => new
        //        {
        //            Id = table.Column<long>(type: "bigint", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_tags", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "users",
        //        schema: "public",
        //        columns: table => new
        //        {
        //            Id = table.Column<long>(type: "bigint", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
        //            Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
        //            PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
        //            DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
        //            Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
        //            AvatarPath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
        //            RegisteredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
        //            LastLogin = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_users", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "apps",
        //        schema: "public",
        //        columns: table => new
        //        {
        //            Id = table.Column<long>(type: "bigint", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            DeveloperId = table.Column<long>(type: "bigint", nullable: true),
        //            CategoryId = table.Column<long>(type: "bigint", nullable: true),
        //            Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
        //            Slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
        //            ShortDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
        //            FullDescription = table.Column<string>(type: "text", nullable: true),
        //            Price = table.Column<decimal>(type: "numeric", nullable: false),
        //            Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
        //            IsPublished = table.Column<bool>(type: "boolean", nullable: false),
        //            PublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
        //            AverageRating = table.Column<decimal>(type: "numeric", nullable: false),
        //            TotalReviews = table.Column<int>(type: "integer", nullable: false),
        //            Metadata = table.Column<JsonDocument>(type: "jsonb", nullable: true),
        //            CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
        //            UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_apps", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_apps_categories_CategoryId",
        //                column: x => x.CategoryId,
        //                principalSchema: "public",
        //                principalTable: "categories",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.SetNull);
        //            table.ForeignKey(
        //                name: "FK_apps_developers_DeveloperId",
        //                column: x => x.DeveloperId,
        //                principalSchema: "public",
        //                principalTable: "developers",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.SetNull);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "audit_logs",
        //        schema: "public",
        //        columns: table => new
        //        {
        //            Id = table.Column<long>(type: "bigint", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            EntityType = table.Column<string>(type: "text", nullable: true),
        //            EntityId = table.Column<long>(type: "bigint", nullable: true),
        //            Action = table.Column<string>(type: "text", nullable: true),
        //            ActorUserId = table.Column<long>(type: "bigint", nullable: true),
        //            Payload = table.Column<JsonDocument>(type: "jsonb", nullable: true),
        //            CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_audit_logs", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_audit_logs_users_ActorUserId",
        //                column: x => x.ActorUserId,
        //                principalSchema: "public",
        //                principalTable: "users",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.SetNull);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "app_icons",
        //        schema: "public",
        //        columns: table => new
        //        {
        //            Id = table.Column<long>(type: "bigint", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            AppId = table.Column<long>(type: "bigint", nullable: false),
        //            FilePath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
        //            UploadedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_app_icons", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_app_icons_apps_AppId",
        //                column: x => x.AppId,
        //                principalSchema: "public",
        //                principalTable: "apps",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "app_tags",
        //        schema: "public",
        //        columns: table => new
        //        {
        //            AppId = table.Column<long>(type: "bigint", nullable: false),
        //            TagId = table.Column<long>(type: "bigint", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_app_tags", x => new { x.AppId, x.TagId });
        //            table.ForeignKey(
        //                name: "FK_app_tags_apps_AppId",
        //                column: x => x.AppId,
        //                principalSchema: "public",
        //                principalTable: "apps",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_app_tags_tags_TagId",
        //                column: x => x.TagId,
        //                principalSchema: "public",
        //                principalTable: "tags",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "app_versions",
        //        schema: "public",
        //        columns: table => new
        //        {
        //            Id = table.Column<long>(type: "bigint", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            AppId = table.Column<long>(type: "bigint", nullable: false),
        //            Version = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
        //            Platform = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
        //            PackageId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
        //            ReleaseNotes = table.Column<string>(type: "text", nullable: true),
        //            FileSizeBytes = table.Column<long>(type: "bigint", nullable: true),
        //            MinOsVersion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
        //            DownloadUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
        //            Checksum = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
        //            UploadedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
        //            IsActive = table.Column<bool>(type: "boolean", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_app_versions", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_app_versions_apps_AppId",
        //                column: x => x.AppId,
        //                principalSchema: "public",
        //                principalTable: "apps",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "reviews",
        //        schema: "public",
        //        columns: table => new
        //        {
        //            Id = table.Column<long>(type: "bigint", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            AppId = table.Column<long>(type: "bigint", nullable: false),
        //            UserId = table.Column<long>(type: "bigint", nullable: true),
        //            Title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
        //            Body = table.Column<string>(type: "text", nullable: true),
        //            Rating = table.Column<short>(type: "smallint", nullable: false),
        //            IsApproved = table.Column<bool>(type: "boolean", nullable: false),
        //            CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
        //            UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_reviews", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_reviews_apps_AppId",
        //                column: x => x.AppId,
        //                principalSchema: "public",
        //                principalTable: "apps",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_reviews_users_UserId",
        //                column: x => x.UserId,
        //                principalSchema: "public",
        //                principalTable: "users",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.SetNull);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "app_screenshots",
        //        schema: "public",
        //        columns: table => new
        //        {
        //            Id = table.Column<long>(type: "bigint", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            AppId = table.Column<long>(type: "bigint", nullable: false),
        //            VersionId = table.Column<long>(type: "bigint", nullable: true),
        //            FilePath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
        //            SortOrder = table.Column<int>(type: "integer", nullable: false),
        //            Caption = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_app_screenshots", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_app_screenshots_app_versions_VersionId",
        //                column: x => x.VersionId,
        //                principalSchema: "public",
        //                principalTable: "app_versions",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_app_screenshots_apps_AppId",
        //                column: x => x.AppId,
        //                principalSchema: "public",
        //                principalTable: "apps",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "downloads",
        //        schema: "public",
        //        columns: table => new
        //        {
        //            Id = table.Column<long>(type: "bigint", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            AppId = table.Column<long>(type: "bigint", nullable: false),
        //            VersionId = table.Column<long>(type: "bigint", nullable: true),
        //            UserId = table.Column<long>(type: "bigint", nullable: true),
        //            IpAddress = table.Column<string>(type: "text", nullable: true),
        //            Platform = table.Column<string>(type: "text", nullable: true),
        //            CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_downloads", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_downloads_app_versions_VersionId",
        //                column: x => x.VersionId,
        //                principalSchema: "public",
        //                principalTable: "app_versions",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_downloads_apps_AppId",
        //                column: x => x.AppId,
        //                principalSchema: "public",
        //                principalTable: "apps",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_downloads_users_UserId",
        //                column: x => x.UserId,
        //                principalSchema: "public",
        //                principalTable: "users",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.SetNull);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "purchases",
        //        schema: "public",
        //        columns: table => new
        //        {
        //            Id = table.Column<long>(type: "bigint", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            UserId = table.Column<long>(type: "bigint", nullable: false),
        //            AppId = table.Column<long>(type: "bigint", nullable: false),
        //            VersionId = table.Column<long>(type: "bigint", nullable: true),
        //            PricePaid = table.Column<decimal>(type: "numeric", nullable: false),
        //            Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
        //            PaymentProvider = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
        //            ProviderTransactionId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
        //            Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
        //            PurchasedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_purchases", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_purchases_app_versions_VersionId",
        //                column: x => x.VersionId,
        //                principalSchema: "public",
        //                principalTable: "app_versions",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_purchases_apps_AppId",
        //                column: x => x.AppId,
        //                principalSchema: "public",
        //                principalTable: "apps",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_purchases_users_UserId",
        //                column: x => x.UserId,
        //                principalSchema: "public",
        //                principalTable: "users",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "reports",
        //        schema: "public",
        //        columns: table => new
        //        {
        //            Id = table.Column<long>(type: "bigint", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            ReporterUserId = table.Column<long>(type: "bigint", nullable: true),
        //            AppId = table.Column<long>(type: "bigint", nullable: true),
        //            ReviewId = table.Column<long>(type: "bigint", nullable: true),
        //            Reason = table.Column<string>(type: "text", nullable: true),
        //            Details = table.Column<string>(type: "text", nullable: true),
        //            CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
        //            Resolved = table.Column<bool>(type: "boolean", nullable: false),
        //            ResolvedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_reports", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_reports_apps_AppId",
        //                column: x => x.AppId,
        //                principalSchema: "public",
        //                principalTable: "apps",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_reports_reviews_ReviewId",
        //                column: x => x.ReviewId,
        //                principalSchema: "public",
        //                principalTable: "reviews",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Restrict);
        //            table.ForeignKey(
        //                name: "FK_reports_users_ReporterUserId",
        //                column: x => x.ReporterUserId,
        //                principalSchema: "public",
        //                principalTable: "users",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.SetNull);
        //        });

        //    migrationBuilder.CreateIndex(
        //        name: "IX_app_icons_AppId",
        //        schema: "public",
        //        table: "app_icons",
        //        column: "AppId",
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "idx_screenshots_app",
        //        schema: "public",
        //        table: "app_screenshots",
        //        column: "AppId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_app_screenshots_VersionId",
        //        schema: "public",
        //        table: "app_screenshots",
        //        column: "VersionId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_app_tags_TagId",
        //        schema: "public",
        //        table: "app_tags",
        //        column: "TagId");

        //    migrationBuilder.CreateIndex(
        //        name: "idx_app_versions_app",
        //        schema: "public",
        //        table: "app_versions",
        //        column: "AppId");

        //    migrationBuilder.CreateIndex(
        //        name: "idx_apps_category",
        //        schema: "public",
        //        table: "apps",
        //        column: "CategoryId");

        //    migrationBuilder.CreateIndex(
        //        name: "idx_apps_developer",
        //        schema: "public",
        //        table: "apps",
        //        column: "DeveloperId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_apps_Slug",
        //        schema: "public",
        //        table: "apps",
        //        column: "Slug",
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_audit_logs_ActorUserId",
        //        schema: "public",
        //        table: "audit_logs",
        //        column: "ActorUserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_categories_ParentId",
        //        schema: "public",
        //        table: "categories",
        //        column: "ParentId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_categories_Slug",
        //        schema: "public",
        //        table: "categories",
        //        column: "Slug",
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "idx_downloads_app",
        //        schema: "public",
        //        table: "downloads",
        //        column: "AppId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_downloads_UserId",
        //        schema: "public",
        //        table: "downloads",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_downloads_VersionId",
        //        schema: "public",
        //        table: "downloads",
        //        column: "VersionId");

        //    migrationBuilder.CreateIndex(
        //        name: "idx_purchases_user",
        //        schema: "public",
        //        table: "purchases",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_purchases_AppId",
        //        schema: "public",
        //        table: "purchases",
        //        column: "AppId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_purchases_VersionId",
        //        schema: "public",
        //        table: "purchases",
        //        column: "VersionId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_reports_AppId",
        //        schema: "public",
        //        table: "reports",
        //        column: "AppId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_reports_ReporterUserId",
        //        schema: "public",
        //        table: "reports",
        //        column: "ReporterUserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_reports_ReviewId",
        //        schema: "public",
        //        table: "reports",
        //        column: "ReviewId");

        //    migrationBuilder.CreateIndex(
        //        name: "idx_reviews_app",
        //        schema: "public",
        //        table: "reviews",
        //        column: "AppId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_reviews_UserId",
        //        schema: "public",
        //        table: "reviews",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_tags_Name",
        //        schema: "public",
        //        table: "tags",
        //        column: "Name",
        //        unique: true);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_users_Email",
        //        schema: "public",
        //        table: "users",
        //        column: "Email",
        //        unique: true);
        //}

        //protected override void Down(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.DropTable(
        //        name: "app_icons",
        //        schema: "public");

        //    migrationBuilder.DropTable(
        //        name: "app_screenshots",
        //        schema: "public");

        //    migrationBuilder.DropTable(
        //        name: "app_tags",
        //        schema: "public");

        //    migrationBuilder.DropTable(
        //        name: "audit_logs",
        //        schema: "public");

        //    migrationBuilder.DropTable(
        //        name: "downloads",
        //        schema: "public");

        //    migrationBuilder.DropTable(
        //        name: "purchases",
        //        schema: "public");

        //    migrationBuilder.DropTable(
        //        name: "reports",
        //        schema: "public");

        //    migrationBuilder.DropTable(
        //        name: "tags",
        //        schema: "public");

        //    migrationBuilder.DropTable(
        //        name: "app_versions",
        //        schema: "public");

        //    migrationBuilder.DropTable(
        //        name: "reviews",
        //        schema: "public");

        //    migrationBuilder.DropTable(
        //        name: "apps",
        //        schema: "public");

        //    migrationBuilder.DropTable(
        //        name: "users",
        //        schema: "public");

        //    migrationBuilder.DropTable(
        //        name: "categories",
        //        schema: "public");

        //    migrationBuilder.DropTable(
        //        name: "developers",
        //        schema: "public");
        }
    }
}
