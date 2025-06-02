using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soditech.IntelPrev.Preventions.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                schema: "preventions",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId",
                schema: "preventions",
                table: "Users",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tenants_TenantId",
                schema: "preventions",
                table: "Users",
                column: "TenantId",
                principalSchema: "preventions",
                principalTable: "Tenants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_TenantId",
                schema: "preventions",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TenantId",
                schema: "preventions",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "preventions",
                table: "Users");
        }
    }
}
