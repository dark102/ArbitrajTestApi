using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ArbitrajTestApi.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArbitrageData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QuarterFutureSymbol = table.Column<string>(type: "text", nullable: false),
                    QuarterFuturePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    BiQuarterFutureSymbol = table.Column<string>(type: "text", nullable: false),
                    BiQuarterFuturePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    PriceDifference = table.Column<decimal>(type: "numeric", nullable: false),
                    PercentageDifference = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArbitrageData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuturesPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Symbol = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuturesPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    message = table.Column<string>(type: "text", nullable: false),
                    messagetemplate = table.Column<string>(type: "text", nullable: false),
                    message_template = table.Column<string>(type: "text", nullable: false),
                    level = table.Column<string>(type: "varchar(10)", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    exception = table.Column<string>(type: "text", nullable: false),
                    properties = table.Column<string>(type: "text", nullable: false),
                    logevent = table.Column<string>(type: "jsonb", nullable: false),
                    log_event = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TrackedPairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuarterFutureSymbol = table.Column<string>(type: "text", nullable: false),
                    BiQuarterFutureSymbol = table.Column<string>(type: "text", nullable: false),
                    isNew = table.Column<bool>(type: "boolean", nullable: false),
                    LastDateOfEntry = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedPairs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArbitrageData_Timestamp",
                table: "ArbitrageData",
                column: "Timestamp",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_FuturesPrice_Timestamp",
                table: "FuturesPrices",
                column: "Timestamp",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Level",
                table: "logs",
                column: "level");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Timestamp",
                table: "logs",
                column: "timestamp",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_TrackedPairs_QuarterFutureSymbol_BiQuarterFutureSymbol",
                table: "TrackedPairs",
                columns: new[] { "QuarterFutureSymbol", "BiQuarterFutureSymbol" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArbitrageData");

            migrationBuilder.DropTable(
                name: "FuturesPrices");

            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropTable(
                name: "TrackedPairs");
        }
    }
}
