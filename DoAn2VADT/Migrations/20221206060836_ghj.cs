using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn2VADT.Migrations
{
    public partial class ghj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Imports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Imports_AccountId",
                table: "Imports",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Imports_Accounts_AccountId",
                table: "Imports",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imports_Accounts_AccountId",
                table: "Imports");

            migrationBuilder.DropIndex(
                name: "IX_Imports_AccountId",
                table: "Imports");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Imports");
        }
    }
}
