using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn2VADT.Migrations
{
    public partial class sssss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportDetails_Import_ImportId",
                table: "ImportDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Import",
                table: "Import");

            migrationBuilder.RenameTable(
                name: "Import",
                newName: "Imports");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Imports",
                table: "Imports",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportDetails_Imports_ImportId",
                table: "ImportDetails",
                column: "ImportId",
                principalTable: "Imports",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportDetails_Imports_ImportId",
                table: "ImportDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Imports",
                table: "Imports");

            migrationBuilder.RenameTable(
                name: "Imports",
                newName: "Import");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Import",
                table: "Import",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportDetails_Import_ImportId",
                table: "ImportDetails",
                column: "ImportId",
                principalTable: "Import",
                principalColumn: "Id");
        }
    }
}
