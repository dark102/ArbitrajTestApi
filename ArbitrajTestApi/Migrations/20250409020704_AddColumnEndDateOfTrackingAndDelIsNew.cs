using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArbitrajTestApi.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnEndDateOfTrackingAndDelIsNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isNew",
                table: "TrackedPairs");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateOfTracking",
                table: "TrackedPairs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_TrackedPairs_EndDateOfTracking",
                table: "TrackedPairs",
                column: "EndDateOfTracking",
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrackedPairs_EndDateOfTracking",
                table: "TrackedPairs");

            migrationBuilder.DropColumn(
                name: "EndDateOfTracking",
                table: "TrackedPairs");

            migrationBuilder.AddColumn<bool>(
                name: "isNew",
                table: "TrackedPairs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
