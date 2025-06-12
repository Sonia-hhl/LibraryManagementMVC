using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement2.Migrations
{
    /// <inheritdoc />
    public partial class UpdateYourModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "Reviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MemberId",
                table: "Reviews",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Members_MemberId",
                table: "Reviews",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Members_MemberId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_MemberId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Reviews");
        }
    }
}
