using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Appliances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WattMin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WattMax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WattTypical = table.Column<decimal>(type: "decimal(8,1)", precision: 8, scale: 1, nullable: false),
                    HoursPerDay = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: false),
                    CalibratedHoursPerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CalibrationFactor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CalibrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Tip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShellyDeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appliances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appliances_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeterReadings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReadingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DayReading = table.Column<decimal>(type: "decimal(10,1)", precision: 10, scale: 1, nullable: false),
                    NightReading = table.Column<decimal>(type: "decimal(10,1)", precision: 10, scale: 1, nullable: true),
                    IsTwoZone = table.Column<bool>(type: "bit", nullable: false),
                    InputMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterReadings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TariffSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SingleRate = table.Column<decimal>(type: "decimal(6,4)", precision: 6, scale: 4, nullable: false),
                    DayRate = table.Column<decimal>(type: "decimal(6,4)", precision: 6, scale: 4, nullable: false),
                    NightRate = table.Column<decimal>(type: "decimal(6,4)", precision: 6, scale: 4, nullable: false),
                    IsManualOverride = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TariffSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TariffSettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillingPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromReadingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToReadingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PeriodStart = table.Column<DateOnly>(type: "date", nullable: false),
                    PeriodEnd = table.Column<DateOnly>(type: "date", nullable: false),
                    DayConsumption = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NightConsumption = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalConsumption = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DayAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NightAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ExplainedPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExplainedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CalibrationFactor = table.Column<decimal>(type: "decimal(6,4)", precision: 6, scale: 4, nullable: false),
                    TariffSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillingPeriods_MeterReadings_FromReadingId",
                        column: x => x.FromReadingId,
                        principalTable: "MeterReadings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BillingPeriods_MeterReadings_ToReadingId",
                        column: x => x.ToReadingId,
                        principalTable: "MeterReadings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BillingPeriods_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appliances_UserId",
                table: "Appliances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BillingPeriods_FromReadingId",
                table: "BillingPeriods",
                column: "FromReadingId");

            migrationBuilder.CreateIndex(
                name: "IX_BillingPeriods_ToReadingId",
                table: "BillingPeriods",
                column: "ToReadingId");

            migrationBuilder.CreateIndex(
                name: "IX_BillingPeriods_UserId",
                table: "BillingPeriods",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterReadings_UserId",
                table: "MeterReadings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TariffSettings_UserId",
                table: "TariffSettings",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appliances");

            migrationBuilder.DropTable(
                name: "BillingPeriods");

            migrationBuilder.DropTable(
                name: "TariffSettings");

            migrationBuilder.DropTable(
                name: "MeterReadings");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
