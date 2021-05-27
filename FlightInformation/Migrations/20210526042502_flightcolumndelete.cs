using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightInformation.Migrations
{
    public partial class flightcolumndelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlightNumber",
                table: "Flight");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlightNumber",
                table: "Flight",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
