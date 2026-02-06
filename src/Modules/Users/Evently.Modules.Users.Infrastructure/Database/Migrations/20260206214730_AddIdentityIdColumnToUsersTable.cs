using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Modules.Users.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityIdColumnToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "identity_id",
                schema: "users",
                table: "users",
                type: "uuid",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "ix_users_identity_id",
                schema: "users",
                table: "users",
                column: "identity_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_identity_id",
                schema: "users",
                table: "users");

            migrationBuilder.DropColumn(
                name: "identity_id",
                schema: "users",
                table: "users");
        }
    }
}
