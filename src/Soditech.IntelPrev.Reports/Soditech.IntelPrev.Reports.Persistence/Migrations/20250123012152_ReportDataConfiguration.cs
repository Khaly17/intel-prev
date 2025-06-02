using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soditech.IntelPrev.Reports.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReportDataConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportDatas_Reports_ReportId",
                schema: "reports",
                table: "ReportDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportDatas_Tenants_TenantId",
                schema: "reports",
                table: "ReportDatas");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                schema: "reports",
                table: "ReportDatas",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "reports",
                table: "ReportDatas",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportDatas_Reports_ReportId",
                schema: "reports",
                table: "ReportDatas",
                column: "ReportId",
                principalSchema: "reports",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportDatas_Tenants_TenantId",
                schema: "reports",
                table: "ReportDatas",
                column: "TenantId",
                principalSchema: "reports",
                principalTable: "Tenants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportDatas_Reports_ReportId",
                schema: "reports",
                table: "ReportDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportDatas_Tenants_TenantId",
                schema: "reports",
                table: "ReportDatas");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                schema: "reports",
                table: "ReportDatas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "reports",
                table: "ReportDatas",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportDatas_Reports_ReportId",
                schema: "reports",
                table: "ReportDatas",
                column: "ReportId",
                principalSchema: "reports",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportDatas_Tenants_TenantId",
                schema: "reports",
                table: "ReportDatas",
                column: "TenantId",
                principalSchema: "reports",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
