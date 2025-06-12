using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement2.Migrations
{
    /// <inheritdoc />
    public partial class UpdateYourModel4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxReserveNum",
                table: "Members",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxReserveNum",
                table: "Members");
        }
    }
}
