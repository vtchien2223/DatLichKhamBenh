using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nhom4_QLBA_API.Migrations
{
    /// <inheritdoc />
    public partial class addRolePermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanManageAppointmentDetails",
                schema: "identity",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManageAppointments",
                schema: "identity",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManageDoctors",
                schema: "identity",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManagePatients",
                schema: "identity",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManagePosts",
                schema: "identity",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManageServices",
                schema: "identity",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanManageSpecialty",
                schema: "identity",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanManageAppointmentDetails",
                schema: "identity",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "CanManageAppointments",
                schema: "identity",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "CanManageDoctors",
                schema: "identity",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "CanManagePatients",
                schema: "identity",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "CanManagePosts",
                schema: "identity",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "CanManageServices",
                schema: "identity",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "CanManageSpecialty",
                schema: "identity",
                table: "AspNetRoles");
        }
    }
}
