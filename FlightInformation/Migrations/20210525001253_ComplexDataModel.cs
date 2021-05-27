using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightInformation.Migrations
{
    public partial class ComplexDataModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_PassengerID",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "TicketID",
                table: "Ticket");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket",
                columns: new[] { "PassengerID", "FlightID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket");

            migrationBuilder.AddColumn<int>(
                name: "TicketID",
                table: "Ticket",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket",
                column: "TicketID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_PassengerID",
                table: "Ticket",
                column: "PassengerID");
        }
    }
}
