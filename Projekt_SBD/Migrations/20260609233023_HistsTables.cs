using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt_SBD.Migrations
{
    public partial class HistsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsedAmount",
                table: "Supplies_HIST",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsedAmount",
                table: "Supplies_HIST");
        }
    }
}
