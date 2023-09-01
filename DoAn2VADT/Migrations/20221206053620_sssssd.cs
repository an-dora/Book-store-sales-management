using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn2VADT.Migrations
{
    public partial class sssssd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreateUserId",
                table: "Orders",
                column: "CreateUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Accounts_CreateUserId",
                table: "Orders",
                column: "CreateUserId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Accounts_CreateUserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CreateUserId",
                table: "Orders");
        }
    }
}
