using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soditech.IntelPrev.Preventions.Persistence.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "preventions");

        migrationBuilder.CreateTable(
            name: "Tenants",
            schema: "preventions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                DisplayName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tenants", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            schema: "preventions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Username = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Buildings",
            schema: "preventions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                NumberOfFloors = table.Column<int>(type: "int", nullable: false),
                HasDAE = table.Column<bool>(type: "bit", nullable: false),
                HasFirstAidKits = table.Column<bool>(type: "bit", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                UpdaterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Buildings", x => x.Id);
                table.ForeignKey(
                    name: "FK_Buildings_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "preventions",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Buildings_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Buildings_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Buildings_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Campaigns",
            schema: "preventions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                EndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                UpdaterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Campaigns", x => x.Id);
                table.ForeignKey(
                    name: "FK_Campaigns_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "preventions",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Campaigns_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Campaigns_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Campaigns_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "CommitteeMembers",
            schema: "preventions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                Roles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                UpdaterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CommitteeMembers", x => x.Id);
                table.ForeignKey(
                    name: "FK_CommitteeMembers_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "preventions",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_CommitteeMembers_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_CommitteeMembers_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_CommitteeMembers_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "MedicalContacts",
            schema: "preventions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                UpdaterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MedicalContacts", x => x.Id);
                table.ForeignKey(
                    name: "FK_MedicalContacts_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "preventions",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_MedicalContacts_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_MedicalContacts_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_MedicalContacts_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "StaticContents",
            schema: "preventions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                UpdaterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_StaticContents", x => x.Id);
                table.ForeignKey(
                    name: "FK_StaticContents_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "preventions",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_StaticContents_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_StaticContents_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_StaticContents_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Equipments",
            schema: "preventions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                LastInspectionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                NextInspectionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                UpdaterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Equipments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Equipments_Buildings_BuildingId",
                    column: x => x.BuildingId,
                    principalSchema: "preventions",
                    principalTable: "Buildings",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Equipments_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "preventions",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Equipments_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Equipments_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Equipments_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Statistics",
            schema: "preventions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Value = table.Column<float>(type: "real", nullable: false),
                Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                UpdaterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Statistics", x => x.Id);
                table.ForeignKey(
                    name: "FK_Statistics_Campaigns_CampaignId",
                    column: x => x.CampaignId,
                    principalSchema: "preventions",
                    principalTable: "Campaigns",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Statistics_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "preventions",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Statistics_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Statistics_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Statistics_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Events",
            schema: "preventions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                EndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                OrganizerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                UpdaterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Events", x => x.Id);
                table.ForeignKey(
                    name: "FK_Events_CommitteeMembers_OrganizerId",
                    column: x => x.OrganizerId,
                    principalSchema: "preventions",
                    principalTable: "CommitteeMembers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Events_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "preventions",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Events_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Events_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Events_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Buildings_CreatorId",
            schema: "preventions",
            table: "Buildings",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_Buildings_DeleterId",
            schema: "preventions",
            table: "Buildings",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_Buildings_TenantId",
            schema: "preventions",
            table: "Buildings",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Buildings_UpdaterId",
            schema: "preventions",
            table: "Buildings",
            column: "UpdaterId");

        migrationBuilder.CreateIndex(
            name: "IX_Campaigns_CreatorId",
            schema: "preventions",
            table: "Campaigns",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_Campaigns_DeleterId",
            schema: "preventions",
            table: "Campaigns",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_Campaigns_TenantId",
            schema: "preventions",
            table: "Campaigns",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Campaigns_UpdaterId",
            schema: "preventions",
            table: "Campaigns",
            column: "UpdaterId");

        migrationBuilder.CreateIndex(
            name: "IX_CommitteeMembers_CreatorId",
            schema: "preventions",
            table: "CommitteeMembers",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_CommitteeMembers_DeleterId",
            schema: "preventions",
            table: "CommitteeMembers",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_CommitteeMembers_TenantId",
            schema: "preventions",
            table: "CommitteeMembers",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_CommitteeMembers_UpdaterId",
            schema: "preventions",
            table: "CommitteeMembers",
            column: "UpdaterId");

        migrationBuilder.CreateIndex(
            name: "IX_Equipments_BuildingId",
            schema: "preventions",
            table: "Equipments",
            column: "BuildingId");

        migrationBuilder.CreateIndex(
            name: "IX_Equipments_CreatorId",
            schema: "preventions",
            table: "Equipments",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_Equipments_DeleterId",
            schema: "preventions",
            table: "Equipments",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_Equipments_TenantId",
            schema: "preventions",
            table: "Equipments",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Equipments_UpdaterId",
            schema: "preventions",
            table: "Equipments",
            column: "UpdaterId");

        migrationBuilder.CreateIndex(
            name: "IX_Events_CreatorId",
            schema: "preventions",
            table: "Events",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_Events_DeleterId",
            schema: "preventions",
            table: "Events",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_Events_OrganizerId",
            schema: "preventions",
            table: "Events",
            column: "OrganizerId");

        migrationBuilder.CreateIndex(
            name: "IX_Events_TenantId",
            schema: "preventions",
            table: "Events",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Events_UpdaterId",
            schema: "preventions",
            table: "Events",
            column: "UpdaterId");

        migrationBuilder.CreateIndex(
            name: "IX_MedicalContacts_CreatorId",
            schema: "preventions",
            table: "MedicalContacts",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_MedicalContacts_DeleterId",
            schema: "preventions",
            table: "MedicalContacts",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_MedicalContacts_TenantId",
            schema: "preventions",
            table: "MedicalContacts",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_MedicalContacts_UpdaterId",
            schema: "preventions",
            table: "MedicalContacts",
            column: "UpdaterId");

        migrationBuilder.CreateIndex(
            name: "IX_StaticContents_CreatorId",
            schema: "preventions",
            table: "StaticContents",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_StaticContents_DeleterId",
            schema: "preventions",
            table: "StaticContents",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_StaticContents_TenantId",
            schema: "preventions",
            table: "StaticContents",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_StaticContents_UpdaterId",
            schema: "preventions",
            table: "StaticContents",
            column: "UpdaterId");

        migrationBuilder.CreateIndex(
            name: "IX_Statistics_CampaignId",
            schema: "preventions",
            table: "Statistics",
            column: "CampaignId");

        migrationBuilder.CreateIndex(
            name: "IX_Statistics_CreatorId",
            schema: "preventions",
            table: "Statistics",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_Statistics_DeleterId",
            schema: "preventions",
            table: "Statistics",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_Statistics_TenantId",
            schema: "preventions",
            table: "Statistics",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Statistics_UpdaterId",
            schema: "preventions",
            table: "Statistics",
            column: "UpdaterId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Equipments",
            schema: "preventions");

        migrationBuilder.DropTable(
            name: "Events",
            schema: "preventions");

        migrationBuilder.DropTable(
            name: "MedicalContacts",
            schema: "preventions");

        migrationBuilder.DropTable(
            name: "StaticContents",
            schema: "preventions");

        migrationBuilder.DropTable(
            name: "Statistics",
            schema: "preventions");

        migrationBuilder.DropTable(
            name: "Buildings",
            schema: "preventions");

        migrationBuilder.DropTable(
            name: "CommitteeMembers",
            schema: "preventions");

        migrationBuilder.DropTable(
            name: "Campaigns",
            schema: "preventions");

        migrationBuilder.DropTable(
            name: "Tenants",
            schema: "preventions");

        migrationBuilder.DropTable(
            name: "Users",
            schema: "preventions");
    }
}
