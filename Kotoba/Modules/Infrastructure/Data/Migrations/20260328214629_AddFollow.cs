using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kotoba.Modules.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFollow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("165dc0a5-eab1-4d0f-bcd4-39d5cc8fbf06"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("49350044-188e-4df7-89a8-dc9a56705c33"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("65713a39-4217-4779-890c-28d286ab0895"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("839838a9-2133-43d7-8872-2eb13187ae0b"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("8850cd92-caa4-4732-acd3-ab9437692142"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("ab79eef2-9c0b-4e92-a0cd-c05405de5720"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("e9a20003-ebf3-4661-befb-bc4697a08f61"));

            migrationBuilder.CreateTable(
                name: "Follow",
                columns: table => new
                {
                    FollowerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FollowingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follow", x => new { x.FollowerId, x.FollowingId });
                    table.ForeignKey(
                        name: "FK_Follow_Users_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Follow_Users_FollowingId",
                        column: x => x.FollowingId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ReportCategories",
                columns: new[] { "Id", "Description", "DisplayOrder", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("0f06cb16-a15b-4a7c-a1d2-c11a5e26485a"), null, 5, true, "Misinformation" },
                    { new Guid("6448e822-cae6-4c56-9e65-4664645240fc"), null, 1, true, "Spam" },
                    { new Guid("6c843eda-38bb-459c-ae6f-0a1303fd873a"), null, 7, true, "Other" },
                    { new Guid("7610fc4c-cfae-42e0-a4c6-78e584a3e524"), null, 6, true, "Violence" },
                    { new Guid("7b2d1784-c666-4e27-94e4-dfed97ef6dc6"), null, 4, true, "Harassment" },
                    { new Guid("9c86a07b-d253-4ab4-af05-0c96eeb19276"), null, 2, true, "Hate speech" },
                    { new Guid("defb1601-e68c-4581-8df4-44aa344c1f93"), null, 3, true, "Adult content" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Follow_FollowingId",
                table: "Follow",
                column: "FollowingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Follow");

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("0f06cb16-a15b-4a7c-a1d2-c11a5e26485a"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("6448e822-cae6-4c56-9e65-4664645240fc"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("6c843eda-38bb-459c-ae6f-0a1303fd873a"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("7610fc4c-cfae-42e0-a4c6-78e584a3e524"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("7b2d1784-c666-4e27-94e4-dfed97ef6dc6"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("9c86a07b-d253-4ab4-af05-0c96eeb19276"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("defb1601-e68c-4581-8df4-44aa344c1f93"));

            migrationBuilder.InsertData(
                table: "ReportCategories",
                columns: new[] { "Id", "Description", "DisplayOrder", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("165dc0a5-eab1-4d0f-bcd4-39d5cc8fbf06"), null, 5, true, "Misinformation" },
                    { new Guid("49350044-188e-4df7-89a8-dc9a56705c33"), null, 3, true, "Adult content" },
                    { new Guid("65713a39-4217-4779-890c-28d286ab0895"), null, 2, true, "Hate speech" },
                    { new Guid("839838a9-2133-43d7-8872-2eb13187ae0b"), null, 4, true, "Harassment" },
                    { new Guid("8850cd92-caa4-4732-acd3-ab9437692142"), null, 7, true, "Other" },
                    { new Guid("ab79eef2-9c0b-4e92-a0cd-c05405de5720"), null, 1, true, "Spam" },
                    { new Guid("e9a20003-ebf3-4661-befb-bc4697a08f61"), null, 6, true, "Violence" }
                });
        }
    }
}
