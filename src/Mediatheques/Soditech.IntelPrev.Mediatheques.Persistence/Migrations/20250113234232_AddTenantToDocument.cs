using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soditech.IntelPrev.Mediatheques.Persistence.Migrations;

/// <inheritdoc />
public partial class AddTenantToDocument : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Documents_Users_CreatorId",
            schema: "mediatheques",
            table: "Documents");

        migrationBuilder.AlterColumn<Guid>(
            name: "CreatorId",
            schema: "mediatheques",
            table: "Documents",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "DeletedAt",
            schema: "mediatheques",
            table: "Documents",
            type: "datetimeoffset",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "DeleterId",
            schema: "mediatheques",
            table: "Documents",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "TenantId",
            schema: "mediatheques",
            table: "Documents",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "UpdatedAt",
            schema: "mediatheques",
            table: "Documents",
            type: "datetimeoffset",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "UpdaterId",
            schema: "mediatheques",
            table: "Documents",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Documents_DeleterId",
            schema: "mediatheques",
            table: "Documents",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_Documents_TenantId",
            schema: "mediatheques",
            table: "Documents",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Documents_UpdaterId",
            schema: "mediatheques",
            table: "Documents",
            column: "UpdaterId");

        migrationBuilder.AddForeignKey(
            name: "FK_Documents_Tenants_TenantId",
            schema: "mediatheques",
            table: "Documents",
            column: "TenantId",
            principalSchema: "mediatheques",
            principalTable: "Tenants",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Documents_Users_CreatorId",
            schema: "mediatheques",
            table: "Documents",
            column: "CreatorId",
            principalSchema: "mediatheques",
            principalTable: "Users",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Documents_Users_DeleterId",
            schema: "mediatheques",
            table: "Documents",
            column: "DeleterId",
            principalSchema: "mediatheques",
            principalTable: "Users",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Documents_Users_UpdaterId",
            schema: "mediatheques",
            table: "Documents",
            column: "UpdaterId",
            principalSchema: "mediatheques",
            principalTable: "Users",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Documents_Tenants_TenantId",
            schema: "mediatheques",
            table: "Documents");

        migrationBuilder.DropForeignKey(
            name: "FK_Documents_Users_CreatorId",
            schema: "mediatheques",
            table: "Documents");

        migrationBuilder.DropForeignKey(
            name: "FK_Documents_Users_DeleterId",
            schema: "mediatheques",
            table: "Documents");

        migrationBuilder.DropForeignKey(
            name: "FK_Documents_Users_UpdaterId",
            schema: "mediatheques",
            table: "Documents");

        migrationBuilder.DropIndex(
            name: "IX_Documents_DeleterId",
            schema: "mediatheques",
            table: "Documents");

        migrationBuilder.DropIndex(
            name: "IX_Documents_TenantId",
            schema: "mediatheques",
            table: "Documents");

        migrationBuilder.DropIndex(
            name: "IX_Documents_UpdaterId",
            schema: "mediatheques",
            table: "Documents");

        migrationBuilder.DropColumn(
            name: "DeletedAt",
            schema: "mediatheques",
            table: "Documents");

        migrationBuilder.DropColumn(
            name: "DeleterId",
            schema: "mediatheques",
            table: "Documents");

        migrationBuilder.DropColumn(
            name: "TenantId",
            schema: "mediatheques",
            table: "Documents");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            schema: "mediatheques",
            table: "Documents");

        migrationBuilder.DropColumn(
            name: "UpdaterId",
            schema: "mediatheques",
            table: "Documents");

        migrationBuilder.AlterColumn<Guid>(
            name: "CreatorId",
            schema: "mediatheques",
            table: "Documents",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AddForeignKey(
            name: "FK_Documents_Users_CreatorId",
            schema: "mediatheques",
            table: "Documents",
            column: "CreatorId",
            principalSchema: "mediatheques",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
