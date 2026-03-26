using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kotoba.Modules.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemMessageColumnsToMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystemMessage",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SystemMessageData",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SystemMessageType",
                table: "Messages",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystemMessage",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SystemMessageData",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SystemMessageType",
                table: "Messages");
        }
    }
}
