using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt_SBD.Migrations
{
    /// <inheritdoc />
    public partial class AddHistsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Supplies_HIST",
                columns: table => new
                {
                    HistId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ActionType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ArchiveDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ArchiveUser = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    OriginalSupplyId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Quantity = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    RoomId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplies_HIST", x => x.HistId);
                });

            migrationBuilder.CreateTable(
                name: "Visits_HIST",
                columns: table => new
                {
                    HistId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ActionType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ArchiveDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ArchiveUser = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    OriginalVisitId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Purpose = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Start = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    End = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Cost = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PatientId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    RoomId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DiagnosisId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    WorkerId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits_HIST", x => x.HistId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Supplies_HIST");

            migrationBuilder.DropTable(
                name: "Visits_HIST");
        }
    }
}
