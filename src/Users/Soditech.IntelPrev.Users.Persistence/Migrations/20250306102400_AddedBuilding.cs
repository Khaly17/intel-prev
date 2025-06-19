using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soditech.IntelPrev.Users.Persistence.Migrations;

/// <inheritdoc />
public partial class AddedBuilding : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_UserRoles_UserId",
            schema: "users",
            table: "UserRoles");

        migrationBuilder.AddColumn<Guid>(
            name: "BuildingId",
            schema: "users",
            table: "Users",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "FloorId",
            schema: "users",
            table: "Users",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "Buildings",
            schema: "users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Buildings", x => x.Id);
                table.ForeignKey(
                    name: "FK_Buildings_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "users",
                    principalTable: "Tenants",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Floors",
            schema: "users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Number = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Floors", x => x.Id);
                table.ForeignKey(
                    name: "FK_Floors_Buildings_BuildingId",
                    column: x => x.BuildingId,
                    principalSchema: "users",
                    principalTable: "Buildings",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Floors_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "users",
                    principalTable: "Tenants",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Users_BuildingId",
            schema: "users",
            table: "Users",
            column: "BuildingId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_FloorId",
            schema: "users",
            table: "Users",
            column: "FloorId");

        migrationBuilder.CreateIndex(
            name: "IX_UserRoles_UserId_RoleId_TenantId",
            schema: "users",
            table: "UserRoles",
            columns: new[] { "UserId", "RoleId", "TenantId" },
            unique: true,
            filter: "[TenantId] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_Buildings_TenantId",
            schema: "users",
            table: "Buildings",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Floors_BuildingId",
            schema: "users",
            table: "Floors",
            column: "BuildingId");

        migrationBuilder.CreateIndex(
            name: "IX_Floors_TenantId",
            schema: "users",
            table: "Floors",
            column: "TenantId");

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Buildings_BuildingId",
            schema: "users",
            table: "Users",
            column: "BuildingId",
            principalSchema: "users",
            principalTable: "Buildings",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Floors_FloorId",
            schema: "users",
            table: "Users",
            column: "FloorId",
            principalSchema: "users",
            principalTable: "Floors",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Users_Buildings_BuildingId",
            schema: "users",
            table: "Users");

        migrationBuilder.DropForeignKey(
            name: "FK_Users_Floors_FloorId",
            schema: "users",
            table: "Users");

        migrationBuilder.DropTable(
            name: "Floors",
            schema: "users");

        migrationBuilder.DropTable(
            name: "Buildings",
            schema: "users");

        migrationBuilder.DropIndex(
            name: "IX_Users_BuildingId",
            schema: "users",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Users_FloorId",
            schema: "users",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_UserRoles_UserId_RoleId_TenantId",
            schema: "users",
            table: "UserRoles");

        migrationBuilder.DropColumn(
            name: "BuildingId",
            schema: "users",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "FloorId",
            schema: "users",
            table: "Users");

        migrationBuilder.CreateIndex(
            name: "IX_UserRoles_UserId",
            schema: "users",
            table: "UserRoles",
            column: "UserId");
    }
}
