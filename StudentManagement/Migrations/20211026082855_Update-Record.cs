using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentManagement.Migrations
{
    public partial class UpdateRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Records",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Records_CreatorId",
                table: "Records",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_AspNetUsers_CreatorId",
                table: "Records",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_AspNetUsers_CreatorId",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_Records_CreatorId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Records");
        }
    }
}
