using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gapplus.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedShareHoldersModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Holdings",
                table: "ShareHolderCompanies",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercentageHolding",
                table: "ShareHolderCompanies",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "MeetingNumber",
                table: "MeetingRegistrations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyStatus",
                table: "Companies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ShareHolders",
                columns: table => new
                {
                    ShareHolderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ShareHolderNum = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    emailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Interests = table.Column<string>(type: "TEXT", nullable: true),
                    ConsolidationStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Sessionid = table.Column<string>(type: "TEXT", nullable: true),
                    SessionVersion = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareHolders", x => x.ShareHolderId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShareHolders");

            migrationBuilder.DropColumn(
                name: "Holdings",
                table: "ShareHolderCompanies");

            migrationBuilder.DropColumn(
                name: "PercentageHolding",
                table: "ShareHolderCompanies");

            migrationBuilder.DropColumn(
                name: "MeetingNumber",
                table: "MeetingRegistrations");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyStatus",
                table: "Companies",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
