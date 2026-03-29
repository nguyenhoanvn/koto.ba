using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kotoba.Modules.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStoryRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

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

            migrationBuilder.RenameTable(
                name: "Follow",
                newName: "Follows");

            migrationBuilder.RenameIndex(
                name: "IX_Follow_FollowingId",
                table: "Follows",
                newName: "IX_Follows_FollowingId");

            migrationBuilder.AddColumn<string>(
                name: "Visibility",
                table: "Stories",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "StoryPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryPermissions_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoryPermissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StoryReactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryReactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryReactions_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoryReactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StoryViews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ViewerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ViewedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryViews_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoryViews_Users_ViewerId",
                        column: x => x.ViewerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ReportCategories",
                columns: new[] { "Id", "Description", "DisplayOrder", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("0fba1b1a-6aa0-4d57-89f7-9d3f841b2168"), null, 3, true, "Adult content" },
                    { new Guid("24a6cb8b-eaec-4ddb-860b-288054892026"), null, 7, true, "Other" },
                    { new Guid("5a30027b-dd63-4d5f-ba4e-42ec436fbb18"), null, 4, true, "Harassment" },
                    { new Guid("6066b5cb-e8a3-40bc-8ec9-82298184b919"), null, 5, true, "Misinformation" },
                    { new Guid("bc6b295d-0d4b-4f1b-9aa9-ee699c08778f"), null, 2, true, "Hate speech" },
                    { new Guid("d45efdf3-e288-45ab-bd98-f1f4e4341adb"), null, 1, true, "Spam" },
                    { new Guid("ded0169c-b4f8-4621-aeff-4ea822bf7a77"), null, 6, true, "Violence" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoryPermissions_StoryId_UserId",
                table: "StoryPermissions",
                columns: new[] { "StoryId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoryPermissions_UserId",
                table: "StoryPermissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryReactions_StoryId_UserId",
                table: "StoryReactions",
                columns: new[] { "StoryId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoryReactions_UserId",
                table: "StoryReactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryViews_StoryId_ViewerId",
                table: "StoryViews",
                columns: new[] { "StoryId", "ViewerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoryViews_ViewerId",
                table: "StoryViews",
                column: "ViewerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Users_FollowerId",
                table: "Follows",
                column: "FollowerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Users_FollowingId",
                table: "Follows",
                column: "FollowingId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Users_FollowerId",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Users_FollowingId",
                table: "Follows");

            migrationBuilder.DropTable(
                name: "StoryPermissions");

            migrationBuilder.DropTable(
                name: "StoryReactions");

            migrationBuilder.DropTable(
                name: "StoryViews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Follows",
                table: "Follows");

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("0fba1b1a-6aa0-4d57-89f7-9d3f841b2168"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("24a6cb8b-eaec-4ddb-860b-288054892026"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("5a30027b-dd63-4d5f-ba4e-42ec436fbb18"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("6066b5cb-e8a3-40bc-8ec9-82298184b919"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("bc6b295d-0d4b-4f1b-9aa9-ee699c08778f"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("d45efdf3-e288-45ab-bd98-f1f4e4341adb"));

            migrationBuilder.DeleteData(
                table: "ReportCategories",
                keyColumn: "Id",
                keyValue: new Guid("ded0169c-b4f8-4621-aeff-4ea822bf7a77"));

            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "Stories");

            migrationBuilder.RenameTable(
                name: "Follows",
                newName: "Follow");

            migrationBuilder.RenameIndex(
                name: "IX_Follows_FollowingId",
                table: "Follow",
                newName: "IX_Follow_FollowingId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Follow_Users_FollowerId",
                table: "Follow",
                column: "FollowerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Follow_Users_FollowingId",
                table: "Follow",
                column: "FollowingId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
