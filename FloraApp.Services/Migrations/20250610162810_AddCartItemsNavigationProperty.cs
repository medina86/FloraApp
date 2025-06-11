using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloraApp.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddCartItemsNavigationProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorTheme",
                table: "Reservations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Reservations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TableCount",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeSlot",
                table: "Reservations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "ReservationProposals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "ReservationProposals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ReservationProposals",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminNotes",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ColorTheme",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "TableCount",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "TimeSlot",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "AdminNotes",
                table: "ReservationProposals");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "ReservationProposals");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ReservationProposals");
        }
    }
}
