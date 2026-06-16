using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt_SBD.Migrations
{
    /// <inheritdoc />
    public partial class AddPeselAndHashing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pesel",
                table: "Workers",
                type: "NVARCHAR2(2000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pesel",
                table: "Patients",
                type: "NVARCHAR2(2000)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pesel",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Pesel",
                table: "Patients");
        }
    }
}
