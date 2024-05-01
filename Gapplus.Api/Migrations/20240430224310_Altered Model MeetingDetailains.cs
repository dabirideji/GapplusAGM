using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gapplus.Api.Migrations
{
    /// <inheritdoc />
    public partial class AlteredModelMeetingDetailains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "ShareHolders",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "ShareHolders",
                newName: "PasswordHash");
        }
    }
}
