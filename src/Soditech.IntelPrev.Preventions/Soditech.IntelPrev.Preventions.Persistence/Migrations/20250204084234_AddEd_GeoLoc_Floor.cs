using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soditech.IntelPrev.Preventions.Persistence.Migrations;

/// <inheritdoc />
public partial class AddEd_GeoLoc_Floor : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Location",
            schema: "preventions",
            table: "Equipments");

        migrationBuilder.AlterColumn<Guid>(
            name: "TenantId",
            schema: "preventions",
            table: "Users",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AddColumn<Guid>(
            name: "FloorId",
            schema: "preventions",
            table: "Equipments",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "GeoLocationId",
            schema: "preventions",
            table: "Equipments",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "Floors",
            schema: "preventions",
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
                    principalSchema: "preventions",
                    principalTable: "Buildings",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Floors_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "preventions",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Floors_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Floors_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Floors_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "GeoLocations",
            schema: "preventions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Latitude = table.Column<double>(type: "float", nullable: false),
                Longitude = table.Column<double>(type: "float", nullable: false),
                Altitude = table.Column<double>(type: "float", nullable: true),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FloorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                table.PrimaryKey("PK_GeoLocations", x => x.Id);
                table.ForeignKey(
                    name: "FK_GeoLocations_Buildings_BuildingId",
                    column: x => x.BuildingId,
                    principalSchema: "preventions",
                    principalTable: "Buildings",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_GeoLocations_Equipments_EquipmentId",
                    column: x => x.EquipmentId,
                    principalSchema: "preventions",
                    principalTable: "Equipments",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_GeoLocations_Floors_FloorId",
                    column: x => x.FloorId,
                    principalSchema: "preventions",
                    principalTable: "Floors",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_GeoLocations_Tenants_TenantId",
                    column: x => x.TenantId,
                    principalSchema: "preventions",
                    principalTable: "Tenants",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_GeoLocations_Users_CreatorId",
                    column: x => x.CreatorId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_GeoLocations_Users_DeleterId",
                    column: x => x.DeleterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_GeoLocations_Users_UpdaterId",
                    column: x => x.UpdaterId,
                    principalSchema: "preventions",
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Equipments_FloorId",
            schema: "preventions",
            table: "Equipments",
            column: "FloorId");

        migrationBuilder.CreateIndex(
            name: "IX_Floors_BuildingId",
            schema: "preventions",
            table: "Floors",
            column: "BuildingId");

        migrationBuilder.CreateIndex(
            name: "IX_Floors_CreatorId",
            schema: "preventions",
            table: "Floors",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_Floors_DeleterId",
            schema: "preventions",
            table: "Floors",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_Floors_TenantId",
            schema: "preventions",
            table: "Floors",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_Floors_UpdaterId",
            schema: "preventions",
            table: "Floors",
            column: "UpdaterId");

        migrationBuilder.CreateIndex(
            name: "IX_GeoLocations_BuildingId",
            schema: "preventions",
            table: "GeoLocations",
            column: "BuildingId");

        migrationBuilder.CreateIndex(
            name: "IX_GeoLocations_CreatorId",
            schema: "preventions",
            table: "GeoLocations",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_GeoLocations_DeleterId",
            schema: "preventions",
            table: "GeoLocations",
            column: "DeleterId");

        migrationBuilder.CreateIndex(
            name: "IX_GeoLocations_EquipmentId",
            schema: "preventions",
            table: "GeoLocations",
            column: "EquipmentId",
            unique: true,
            filter: "[EquipmentId] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_GeoLocations_FloorId",
            schema: "preventions",
            table: "GeoLocations",
            column: "FloorId");

        migrationBuilder.CreateIndex(
            name: "IX_GeoLocations_TenantId",
            schema: "preventions",
            table: "GeoLocations",
            column: "TenantId");

        migrationBuilder.CreateIndex(
            name: "IX_GeoLocations_UpdaterId",
            schema: "preventions",
            table: "GeoLocations",
            column: "UpdaterId");

        migrationBuilder.AddForeignKey(
            name: "FK_Equipments_Floors_FloorId",
            schema: "preventions",
            table: "Equipments",
            column: "FloorId",
            principalSchema: "preventions",
            principalTable: "Floors",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Equipments_Floors_FloorId",
            schema: "preventions",
            table: "Equipments");

        migrationBuilder.DropTable(
            name: "GeoLocations",
            schema: "preventions");

        migrationBuilder.DropTable(
            name: "Floors",
            schema: "preventions");

        migrationBuilder.DropIndex(
            name: "IX_Equipments_FloorId",
            schema: "preventions",
            table: "Equipments");

        migrationBuilder.DropColumn(
            name: "FloorId",
            schema: "preventions",
            table: "Equipments");

        migrationBuilder.DropColumn(
            name: "GeoLocationId",
            schema: "preventions",
            table: "Equipments");

        migrationBuilder.AlterColumn<Guid>(
            name: "TenantId",
            schema: "preventions",
            table: "Users",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Location",
            schema: "preventions",
            table: "Equipments",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: false,
            defaultValue: "");
    }
}
