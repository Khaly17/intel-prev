using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soditech.IntelPrev.Reports.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDisplayname_for_mobile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "reports",
                table: "Reports",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                schema: "reports",
                table: "Reports",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "Set the default value.");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                schema: "reports",
                table: "RegisterTypes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "Set the default value.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                schema: "reports",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                schema: "reports",
                table: "RegisterTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "reports",
                table: "Reports",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);
        }
    }
}
