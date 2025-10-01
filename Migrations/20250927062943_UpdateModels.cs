using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoinDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "MembershipLevel",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "LibraryBranchId",
                table: "Books",
                newName: "PublishedYear");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Customers",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Customers",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Customers",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Books",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "Books",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "PublishedYear",
                table: "Books",
                newName: "LibraryBranchId");

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinDate",
                table: "Customers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MembershipLevel",
                table: "Customers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Customers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Customers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
