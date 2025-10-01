using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Data.Migratio
{
    /// <inheritdoc />
    public partial class Update_20250927 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "LibraryBranches");

            migrationBuilder.DropColumn(
                name: "OpenHours",
                table: "LibraryBranches");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "LibraryBranches");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Books");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "LibraryBranches",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LibraryBranches",
                type: "TEXT",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "LibraryBranches",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Customers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "LibraryBranchId",
                table: "Customers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PublishedYear",
                table: "Books",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "ISBN",
                table: "Books",
                type: "TEXT",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LibraryBranchId",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Books",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nationality",
                table: "Authors",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Authors",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Biography",
                table: "Authors",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 1000);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBranches_Name",
                table: "LibraryBranches",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LibraryBranchId",
                table: "Customers",
                column: "LibraryBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ISBN",
                table: "Books",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_Books_LibraryBranchId",
                table: "Books",
                column: "LibraryBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_Email",
                table: "Authors",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_LibraryBranches_LibraryBranchId",
                table: "Books",
                column: "LibraryBranchId",
                principalTable: "LibraryBranches",
                principalColumn: "LibraryBranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_LibraryBranches_LibraryBranchId",
                table: "Customers",
                column: "LibraryBranchId",
                principalTable: "LibraryBranches",
                principalColumn: "LibraryBranchId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_LibraryBranches_LibraryBranchId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_LibraryBranches_LibraryBranchId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_LibraryBranches_Name",
                table: "LibraryBranches");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Email",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_LibraryBranchId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Books_AuthorId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_ISBN",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_LibraryBranchId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Authors_Email",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "LibraryBranches");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "LibraryBranches");

            migrationBuilder.DropColumn(
                name: "LibraryBranchId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "LibraryBranchId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Books");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "LibraryBranches",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "LibraryBranches",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OpenHours",
                table: "LibraryBranches",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "LibraryBranches",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Customers",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customers",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Customers",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "PublishedYear",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ISBN",
                table: "Books",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Books",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Nationality",
                table: "Authors",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Authors",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Biography",
                table: "Authors",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
