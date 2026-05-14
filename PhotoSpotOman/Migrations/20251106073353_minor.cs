using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoSpotOman.Migrations
{
    /// <inheritdoc />
    public partial class minor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpotImages_Users_UploadedBy",
                table: "SpotImages");

            migrationBuilder.AddForeignKey(
                name: "FK_SpotImages_Users_UploadedBy",
                table: "SpotImages",
                column: "UploadedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpotImages_Users_UploadedBy",
                table: "SpotImages");

            migrationBuilder.AddForeignKey(
                name: "FK_SpotImages_Users_UploadedBy",
                table: "SpotImages",
                column: "UploadedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
