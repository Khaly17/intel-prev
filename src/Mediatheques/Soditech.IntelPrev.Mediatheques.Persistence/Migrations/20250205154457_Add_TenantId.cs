using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soditech.IntelPrev.Mediatheques.Persistence.Migrations;

/// <inheritdoc />
public partial class Add_TenantId : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "TenantId",
            schema: "mediatheques",
            table: "Users",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("aaca0be0-e107-4a6c-97bc-1eb670a3f696")); // for existing data

        migrationBuilder.CreateIndex(
            name: "IX_Users_TenantId",
            schema: "mediatheques",
            table: "Users",
            column: "TenantId");

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Tenants_TenantId",
            schema: "mediatheques",
            table: "Users",
            column: "TenantId",
            principalSchema: "mediatheques",
            principalTable: "Tenants",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Users_Tenants_TenantId",
            schema: "mediatheques",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Users_TenantId",
            schema: "mediatheques",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "TenantId",
            schema: "mediatheques",
            table: "Users");
    }
}
