using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kotoba.Modules.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAccountStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add/update columns conditionally to avoid errors on databases where they
            // were created manually or by a partially applied migration.

            // 1) Ensure AccountStatus column exists (nullable for now).
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Users', 'AccountStatus') IS NULL
BEGIN
    ALTER TABLE [dbo].[Users]
        ADD [AccountStatus] nvarchar(40) NULL;
END;
");

            // 2) Backfill existing rows to 'Active' and make the column NOT NULL.
            // This is a separate batch so the previous ALTER is visible at compile time.
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Users', 'AccountStatus') IS NOT NULL
BEGIN
    UPDATE [dbo].[Users]
    SET [AccountStatus] = 'Active'
    WHERE [AccountStatus] IS NULL OR [AccountStatus] = '';

    ALTER TABLE [dbo].[Users]
        ALTER COLUMN [AccountStatus] nvarchar(40) NOT NULL;
END;
");

            // DeactivatedAt: nullable timestamp.
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Users', 'DeactivatedAt') IS NULL
BEGIN
    ALTER TABLE [dbo].[Users]
        ADD [DeactivatedAt] datetime2 NULL;
END;
");

            // DeletedAt: nullable timestamp.
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Users', 'DeletedAt') IS NULL
BEGIN
    ALTER TABLE [dbo].[Users]
        ADD [DeletedAt] datetime2 NULL;
END;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountStatus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeactivatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Users");
        }
    }
}
