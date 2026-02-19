using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Servino.Infa.Db.SqlServer.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class updateRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NoSuggestionReminderAt",
                table: "Requests",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoSuggestionReminderAt",
                table: "Requests");
        }
    }
}
