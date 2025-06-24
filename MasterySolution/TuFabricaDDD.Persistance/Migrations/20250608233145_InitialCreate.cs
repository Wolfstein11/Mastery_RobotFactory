using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuFabricaDDD.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Robots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SerialNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    CurrentLocation_X = table.Column<double>(type: "double precision", nullable: false),
                    CurrentLocation_Y = table.Column<double>(type: "double precision", nullable: false),
                    NetworkLocation_IPAddress = table.Column<IPAddress>(type: "inet", nullable: false),
                    NetworkLocation_AccessPoint_Ssid = table.Column<string>(type: "text", nullable: false),
                    NetworkLocation_AccessPoint_Bssid = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    EquippedCleaningTools = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DetergentLevelPercentage = table.Column<double>(type: "double precision", nullable: true),
                    WaterLevelPercentage = table.Column<double>(type: "double precision", nullable: true),
                    DustbinCapacityPercentage = table.Column<double>(type: "double precision", nullable: true),
                    BatteryLevelPercentage = table.Column<double>(type: "double precision", nullable: true),
                    Locomotion = table.Column<int>(type: "integer", nullable: true),
                    ArmLengthInMeters = table.Column<double>(type: "double precision", nullable: true),
                    PayloadCapacityInKg = table.Column<double>(type: "double precision", nullable: true),
                    MountingPointId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HasManipulators = table.Column<bool>(type: "boolean", nullable: true),
                    OperatingSystemVersion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EquippedTools = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    MaxPayloadCapacityInKg = table.Column<double>(type: "double precision", nullable: true),
                    CurrentPayloadWeightInKg = table.Column<double>(type: "double precision", nullable: true),
                    NavigationSystemType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CurrentCargoDescription = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Robots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RobotStateChangeRecords",
                columns: table => new
                {
                    RobotId = table.Column<Guid>(type: "uuid", nullable: false),
                    OccurringTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OldStatus = table.Column<string>(type: "text", nullable: false),
                    NewStatus = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RobotStateChangeRecords", x => new { x.RobotId, x.OccurringTime });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Robots_SerialNumber",
                table: "Robots",
                column: "SerialNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Robots");

            migrationBuilder.DropTable(
                name: "RobotStateChangeRecords");
        }
    }
}
