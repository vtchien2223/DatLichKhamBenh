using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nhom4_QLBA_API.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentTimeColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "AppointmentDateEnd",
                schema: "identity",
                table: "AppointmentDetails",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AppointmentDateStart",
                schema: "identity",
                table: "AppointmentDetails",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDateEnd",
                schema: "identity",
                table: "AppointmentDetails");

            migrationBuilder.DropColumn(
                name: "AppointmentDateStart",
                schema: "identity",
                table: "AppointmentDetails");
        }
    }
}
