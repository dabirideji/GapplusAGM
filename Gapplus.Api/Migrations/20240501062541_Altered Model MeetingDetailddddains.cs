using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gapplus.Api.Migrations
{
    /// <inheritdoc />
    public partial class AlteredModelMeetingDetailddddains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ShareHolderCompanies_CompanyId",
                table: "ShareHolderCompanies",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareHolderCompanies_Companies_CompanyId",
                table: "ShareHolderCompanies",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareHolderCompanies_Companies_CompanyId",
                table: "ShareHolderCompanies");

            migrationBuilder.DropIndex(
                name: "IX_ShareHolderCompanies_CompanyId",
                table: "ShareHolderCompanies");
        }
    }
}
