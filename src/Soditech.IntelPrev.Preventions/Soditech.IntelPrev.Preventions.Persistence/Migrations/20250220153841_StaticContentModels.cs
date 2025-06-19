using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soditech.IntelPrev.Preventions.Persistence.Migrations;

/// <inheritdoc />
public partial class StaticContentModels : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "FireSecuritySettings",
            schema: "preventions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DefinitionContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                KnownMyEnterpriseContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                FireSecurityServiceContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                FireConsignsContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                FireMaterialsContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                EvacuationCaseContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                table.PrimaryKey("PK_FireSecuritySettings", x => x.Id);
                table.ForeignKey(
                    name: "FK_FireSecuritySettings_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "preventions",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_FireSecuritySettings_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_FireSecuritySettings_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_FireSecuritySettings_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "PreventionSettings",
            schema: "preventions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DefinitionContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                SensibilisationContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                table.PrimaryKey("PK_PreventionSettings", x => x.Id);
                table.ForeignKey(
                    name: "FK_PreventionSettings_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "preventions",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_PreventionSettings_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_PreventionSettings_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_PreventionSettings_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "ProPrevSettings",
            schema: "preventions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RiskAnalysisProtocolContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                AnalysisToolsContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                ActionsOrganizerContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                SitesVisitContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                CseAgendaContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                SecurityQuarterContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                EpiControlContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                DataSheetContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                FirstAidKitContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                HealthFormationContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                MyLibraryContent = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                table.PrimaryKey("PK_ProPrevSettings", x => x.Id);
                table.ForeignKey(
                    name: "FK_ProPrevSettings_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "preventions",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_ProPrevSettings_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_ProPrevSettings_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_ProPrevSettings_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_FireSecuritySettings_CreatorId",
            schema: "preventions",
            table: "FireSecuritySettings",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_FireSecuritySettings_DeleterId",
            schema: "preventions",
            table: "FireSecuritySettings",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_FireSecuritySettings_TenantId",
            schema: "preventions",
            table: "FireSecuritySettings",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_FireSecuritySettings_UpdaterId",
            schema: "preventions",
            table: "FireSecuritySettings",
            column: "UpdaterId");

        migrationBuilder.CreateIndex(
            name: "IX_PreventionSettings_CreatorId",
            schema: "preventions",
            table: "PreventionSettings",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_PreventionSettings_DeleterId",
            schema: "preventions",
            table: "PreventionSettings",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_PreventionSettings_TenantId",
            schema: "preventions",
            table: "PreventionSettings",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_PreventionSettings_UpdaterId",
            schema: "preventions",
            table: "PreventionSettings",
            column: "UpdaterId");

        migrationBuilder.CreateIndex(
            name: "IX_ProPrevSettings_CreatorId",
            schema: "preventions",
            table: "ProPrevSettings",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_ProPrevSettings_DeleterId",
            schema: "preventions",
            table: "ProPrevSettings",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_ProPrevSettings_TenantId",
            schema: "preventions",
            table: "ProPrevSettings",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_ProPrevSettings_UpdaterId",
            schema: "preventions",
            table: "ProPrevSettings",
            column: "UpdaterId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "FireSecuritySettings",
            schema: "preventions");

        migrationBuilder.DropTable(
            name: "PreventionSettings",
            schema: "preventions");

        migrationBuilder.DropTable(
            name: "ProPrevSettings",
            schema: "preventions");
    }
}
