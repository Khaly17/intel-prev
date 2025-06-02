using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soditech.IntelPrev.Preventions.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GatheringPoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GatheringPointId",
                schema: "preventions",
                table: "GeoLocations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GatheringPoints",
                schema: "preventions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeoLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_GatheringPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GatheringPoints_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalSchema: "preventions",
                        principalTable: "Buildings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GatheringPoints_Floors_FloorId",
                        column: x => x.FloorId,
                        principalSchema: "preventions",
                        principalTable: "Floors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GatheringPoints_GeoLocations_GeoLocationId",
                        column: x => x.GeoLocationId,
                        principalSchema: "preventions",
                        principalTable: "GeoLocations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GatheringPoints_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "preventions",
                        principalTable: "Tenants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GatheringPoints_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "preventions",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GatheringPoints_Users_DeleterId",
                        column: x => x.DeleterId,
                        principalSchema: "preventions",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GatheringPoints_Users_UpdaterId",
                        column: x => x.UpdaterId,
                        principalSchema: "preventions",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GatheringPoints_BuildingId",
                schema: "preventions",
                table: "GatheringPoints",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_GatheringPoints_CreatorId",
                schema: "preventions",
                table: "GatheringPoints",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_GatheringPoints_DeleterId",
                schema: "preventions",
                table: "GatheringPoints",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_GatheringPoints_FloorId",
                schema: "preventions",
                table: "GatheringPoints",
                column: "FloorId");

            migrationBuilder.CreateIndex(
                name: "IX_GatheringPoints_GeoLocationId",
                schema: "preventions",
                table: "GatheringPoints",
                column: "GeoLocationId",
                unique: true,
                filter: "[GeoLocationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GatheringPoints_TenantId",
                schema: "preventions",
                table: "GatheringPoints",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GatheringPoints_UpdaterId",
                schema: "preventions",
                table: "GatheringPoints",
                column: "UpdaterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GatheringPoints",
                schema: "preventions");

            migrationBuilder.DropColumn(
                name: "GatheringPointId",
                schema: "preventions",
                table: "GeoLocations");
        }
    }
}
