using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soditech.IntelPrev.Reports.Persistence.Migrations;

/// <inheritdoc />
public partial class AddFieldGroup : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "TenantId",
            schema: "reports",
            table: "Users",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AlterColumn<string>(
            name: "Status",
            schema: "reports",
            table: "Reports",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<string>(
            name: "Content",
            schema: "reports",
            table: "ReportComments",
            type: "nvarchar(1024)",
            maxLength: 1024,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(64)",
            oldMaxLength: 64);

        migrationBuilder.AddColumn<string>(
            name: "Title",
            schema: "reports",
            table: "ReportComments",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            schema: "reports",
            table: "RegisterTypes",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(64)",
            oldMaxLength: 64);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            schema: "reports",
            table: "RegisterFields",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(64)",
            oldMaxLength: 64);

        migrationBuilder.AlterColumn<string>(
            name: "FieldType",
            schema: "reports",
            table: "RegisterFields",
            type: "nvarchar(512)",
            maxLength: 512,
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int",
            oldMaxLength: 512);

        migrationBuilder.AddColumn<string>(
            name: "Description",
            schema: "reports",
            table: "RegisterFields",
            type: "nvarchar(512)",
            maxLength: 512,
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "DisplayOrder",
            schema: "reports",
            table: "RegisterFields",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<Guid>(
            name: "RegisterFieldGroupId",
            schema: "reports",
            table: "RegisterFields",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "RegisterFieldGroups",
            schema: "reports",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                DisplayOrder = table.Column<int>(type: "int", nullable: false),
                RegisterTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                table.PrimaryKey("PK_RegisterFieldGroups", x => x.Id);
                table.ForeignKey(
                    name: "FK_RegisterFieldGroups_RegisterTypes_RegisterTypeId",
                    column: x => x.RegisterTypeId,
                    principalSchema: "reports",
                    principalTable: "RegisterTypes",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_RegisterFieldGroups_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "reports",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_RegisterFieldGroups_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "reports",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_RegisterFieldGroups_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "reports",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_RegisterFieldGroups_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "reports",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Users_TenantId",
            schema: "reports",
            table: "Users",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_RegisterFields_RegisterFieldGroupId",
            schema: "reports",
            table: "RegisterFields",
            column: "RegisterFieldGroupId");

        migrationBuilder.CreateIndex(
            name: "IX_RegisterFieldGroups_CreatorId",
            schema: "reports",
            table: "RegisterFieldGroups",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_RegisterFieldGroups_DeleterId",
            schema: "reports",
            table: "RegisterFieldGroups",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_RegisterFieldGroups_RegisterTypeId",
            schema: "reports",
            table: "RegisterFieldGroups",
            column: "RegisterTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_RegisterFieldGroups_TenantId",
            schema: "reports",
            table: "RegisterFieldGroups",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_RegisterFieldGroups_UpdaterId",
            schema: "reports",
            table: "RegisterFieldGroups",
            column: "UpdaterId");

        migrationBuilder.AddForeignKey(
            name: "FK_RegisterFields_RegisterFieldGroups_RegisterFieldGroupId",
            schema: "reports",
            table: "RegisterFields",
            column: "RegisterFieldGroupId",
            principalSchema: "reports",
            principalTable: "RegisterFieldGroups",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Tenants_TenantId",
            schema: "reports",
            table: "Users",
            column: "TenantId",
            principalSchema: "reports",
            principalTable: "Tenants",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_RegisterFields_RegisterFieldGroups_RegisterFieldGroupId",
            schema: "reports",
            table: "RegisterFields");

        migrationBuilder.DropForeignKey(
            name: "FK_Users_Tenants_TenantId",
            schema: "reports",
            table: "Users");

        migrationBuilder.DropTable(
            name: "RegisterFieldGroups",
            schema: "reports");

        migrationBuilder.DropIndex(
            name: "IX_Users_TenantId",
            schema: "reports",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_RegisterFields_RegisterFieldGroupId",
            schema: "reports",
            table: "RegisterFields");

        migrationBuilder.DropColumn(
            name: "TenantId",
            schema: "reports",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "Title",
            schema: "reports",
            table: "ReportComments");

        migrationBuilder.DropColumn(
            name: "Description",
            schema: "reports",
            table: "RegisterFields");

        migrationBuilder.DropColumn(
            name: "DisplayOrder",
            schema: "reports",
            table: "RegisterFields");

        migrationBuilder.DropColumn(
            name: "RegisterFieldGroupId",
            schema: "reports",
            table: "RegisterFields");

        migrationBuilder.AlterColumn<int>(
            name: "Status",
            schema: "reports",
            table: "Reports",
            type: "int",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "Content",
            schema: "reports",
            table: "ReportComments",
            type: "nvarchar(64)",
            maxLength: 64,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(1024)",
            oldMaxLength: 1024);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            schema: "reports",
            table: "RegisterTypes",
            type: "nvarchar(64)",
            maxLength: 64,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(255)",
            oldMaxLength: 255);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            schema: "reports",
            table: "RegisterFields",
            type: "nvarchar(64)",
            maxLength: 64,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(255)",
            oldMaxLength: 255);

        migrationBuilder.AlterColumn<int>(
            name: "FieldType",
            schema: "reports",
            table: "RegisterFields",
            type: "int",
            maxLength: 512,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(512)",
            oldMaxLength: 512);
    }
}
