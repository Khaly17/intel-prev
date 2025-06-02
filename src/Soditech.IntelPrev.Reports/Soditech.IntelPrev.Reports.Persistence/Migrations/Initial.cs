using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soditech.IntelPrev.Reports.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "reports");

            migrationBuilder.CreateTable(
                name: "Tenants",
                schema: "reports",
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
                schema: "reports",
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
                name: "RegisterTypes",
                schema: "reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_RegisterTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisterTypes_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "reports",
                        principalTable: "Tenants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegisterTypes_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegisterTypes_Users_DeleterId",
                        column: x => x.DeleterId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegisterTypes_Users_UpdaterId",
                        column: x => x.UpdaterId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RegisterFields",
                schema: "reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    FieldType = table.Column<int>(type: "int", maxLength: 512, nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_RegisterFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisterFields_RegisterTypes_RegisterTypeId",
                        column: x => x.RegisterTypeId,
                        principalSchema: "reports",
                        principalTable: "RegisterTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegisterFields_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "reports",
                        principalTable: "Tenants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegisterFields_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegisterFields_Users_DeleterId",
                        column: x => x.DeleterId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegisterFields_Users_UpdaterId",
                        column: x => x.UpdaterId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                schema: "reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_RegisterTypes_RegisterTypeId",
                        column: x => x.RegisterTypeId,
                        principalSchema: "reports",
                        principalTable: "RegisterTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "reports",
                        principalTable: "Tenants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Users_DeleterId",
                        column: x => x.DeleterId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Users_UpdaterId",
                        column: x => x.UpdaterId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReportAttachments",
                schema: "reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ReportAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportAttachments_Reports_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "reports",
                        principalTable: "Reports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportAttachments_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "reports",
                        principalTable: "Tenants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportAttachments_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportAttachments_Users_DeleterId",
                        column: x => x.DeleterId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportAttachments_Users_UpdaterId",
                        column: x => x.UpdaterId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReportComments",
                schema: "reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ReportComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportComments_Reports_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "reports",
                        principalTable: "Reports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportComments_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "reports",
                        principalTable: "Tenants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportComments_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportComments_Users_DeleterId",
                        column: x => x.DeleterId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportComments_Users_UpdaterId",
                        column: x => x.UpdaterId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReportDatas",
                schema: "reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdaterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportDatas_RegisterFields_FieldId",
                        column: x => x.FieldId,
                        principalSchema: "reports",
                        principalTable: "RegisterFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportDatas_Reports_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "reports",
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportDatas_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "reports",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportDatas_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportDatas_Users_DeleterId",
                        column: x => x.DeleterId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportDatas_Users_UpdaterId",
                        column: x => x.UpdaterId,
                        principalSchema: "reports",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisterFields_CreatorId",
                schema: "reports",
                table: "RegisterFields",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterFields_DeleterId",
                schema: "reports",
                table: "RegisterFields",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterFields_RegisterTypeId",
                schema: "reports",
                table: "RegisterFields",
                column: "RegisterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterFields_TenantId",
                schema: "reports",
                table: "RegisterFields",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterFields_UpdaterId",
                schema: "reports",
                table: "RegisterFields",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterTypes_CreatorId",
                schema: "reports",
                table: "RegisterTypes",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterTypes_DeleterId",
                schema: "reports",
                table: "RegisterTypes",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterTypes_TenantId",
                schema: "reports",
                table: "RegisterTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterTypes_UpdaterId",
                schema: "reports",
                table: "RegisterTypes",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportAttachments_CreatorId",
                schema: "reports",
                table: "ReportAttachments",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportAttachments_DeleterId",
                schema: "reports",
                table: "ReportAttachments",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportAttachments_ReportId",
                schema: "reports",
                table: "ReportAttachments",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportAttachments_TenantId",
                schema: "reports",
                table: "ReportAttachments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportAttachments_UpdaterId",
                schema: "reports",
                table: "ReportAttachments",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportComments_CreatorId",
                schema: "reports",
                table: "ReportComments",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportComments_DeleterId",
                schema: "reports",
                table: "ReportComments",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportComments_ReportId",
                schema: "reports",
                table: "ReportComments",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportComments_TenantId",
                schema: "reports",
                table: "ReportComments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportComments_UpdaterId",
                schema: "reports",
                table: "ReportComments",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportDatas_CreatorId",
                schema: "reports",
                table: "ReportDatas",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportDatas_DeleterId",
                schema: "reports",
                table: "ReportDatas",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportDatas_FieldId",
                schema: "reports",
                table: "ReportDatas",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportDatas_ReportId",
                schema: "reports",
                table: "ReportDatas",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportDatas_TenantId",
                schema: "reports",
                table: "ReportDatas",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportDatas_UpdaterId",
                schema: "reports",
                table: "ReportDatas",
                column: "UpdaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CreatorId",
                schema: "reports",
                table: "Reports",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_DeleterId",
                schema: "reports",
                table: "Reports",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_RegisterTypeId",
                schema: "reports",
                table: "Reports",
                column: "RegisterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TenantId",
                schema: "reports",
                table: "Reports",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UpdaterId",
                schema: "reports",
                table: "Reports",
                column: "UpdaterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportAttachments",
                schema: "reports");

            migrationBuilder.DropTable(
                name: "ReportComments",
                schema: "reports");

            migrationBuilder.DropTable(
                name: "ReportDatas",
                schema: "reports");

            migrationBuilder.DropTable(
                name: "RegisterFields",
                schema: "reports");

            migrationBuilder.DropTable(
                name: "Reports",
                schema: "reports");

            migrationBuilder.DropTable(
                name: "RegisterTypes",
                schema: "reports");

            migrationBuilder.DropTable(
                name: "Tenants",
                schema: "reports");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "reports");
        }
    }
}
