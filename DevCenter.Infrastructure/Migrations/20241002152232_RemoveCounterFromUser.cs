using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevCenter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCounterFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Counter",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Counter",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
