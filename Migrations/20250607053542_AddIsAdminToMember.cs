﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement2.Migrations
{
    /// <inheritdoc />
    public partial class AddIsAdminToMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Members",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Members");
        }
    }
}
