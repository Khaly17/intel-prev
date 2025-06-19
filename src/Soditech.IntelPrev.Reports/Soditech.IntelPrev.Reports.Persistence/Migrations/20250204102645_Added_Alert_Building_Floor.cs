using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soditech.IntelPrev.Reports.Persistence.Migrations;

/// <inheritdoc />
public partial class Added_Alert_Building_Floor : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Buildings",
            schema: "reports",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
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
                    principalSchema: "reports",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Buildings_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "reports",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Buildings_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "reports",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Buildings_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "reports",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Floors",
            schema: "reports",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Number = table.Column<int>(type: "int", maxLength: 100, nullable: false),
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
                table.PrimaryKey("PK_Floors", x => x.Id);
                table.ForeignKey(
                    name: "FK_Floors_Buildings_BuildingId",
                    column: x => x.BuildingId,
                    principalSchema: "reports",
                    principalTable: "Buildings",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Floors_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "reports",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Floors_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "reports",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Floors_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "reports",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Floors_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "reports",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Alerts",
            schema: "reports",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Latitude = table.Column<double>(type: "float", nullable: false),
                Longitude = table.Column<double>(type: "float", nullable: false),
                Altitude = table.Column<double>(type: "float", nullable: true),
                BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FloorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                table.PrimaryKey("PK_Alerts", x => x.Id);
                table.ForeignKey(
                    name: "FK_Alerts_Buildings_BuildingId",
                    column: x => x.BuildingId,
                    principalSchema: "reports",
                    principalTable: "Buildings",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Alerts_Floors_FloorId",
                    column: x => x.FloorId,
                    principalSchema: "reports",
                    principalTable: "Floors",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Alerts_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "reports",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Alerts_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "reports",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Alerts_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "reports",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Alerts_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "reports",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Alerts_BuildingId",
            schema: "reports",
            table: "Alerts",
            column: "BuildingId");

        migrationBuilder.CreateIndex(
            name: "IX_Alerts_CreatorId",
            schema: "reports",
            table: "Alerts",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_Alerts_DeleterId",
            schema: "reports",
            table: "Alerts",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_Alerts_FloorId",
            schema: "reports",
            table: "Alerts",
            column: "FloorId");

        migrationBuilder.CreateIndex(
            name: "IX_Alerts_TenantId",
            schema: "reports",
            table: "Alerts",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Alerts_UpdaterId",
            schema: "reports",
            table: "Alerts",
            column: "UpdaterId");

        migrationBuilder.CreateIndex(
            name: "IX_Buildings_CreatorId",
            schema: "reports",
            table: "Buildings",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_Buildings_DeleterId",
            schema: "reports",
            table: "Buildings",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_Buildings_TenantId",
            schema: "reports",
            table: "Buildings",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Buildings_UpdaterId",
            schema: "reports",
            table: "Buildings",
            column: "UpdaterId");

        migrationBuilder.CreateIndex(
            name: "IX_Floors_BuildingId",
            schema: "reports",
            table: "Floors",
            column: "BuildingId");

        migrationBuilder.CreateIndex(
            name: "IX_Floors_CreatorId",
            schema: "reports",
            table: "Floors",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_Floors_DeleterId",
            schema: "reports",
            table: "Floors",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_Floors_TenantId",
            schema: "reports",
            table: "Floors",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Floors_UpdaterId",
            schema: "reports",
            table: "Floors",
            column: "UpdaterId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Alerts",
            schema: "reports");

        migrationBuilder.DropTable(
            name: "Floors",
            schema: "reports");

        migrationBuilder.DropTable(
            name: "Buildings",
            schema: "reports");
    }
}
