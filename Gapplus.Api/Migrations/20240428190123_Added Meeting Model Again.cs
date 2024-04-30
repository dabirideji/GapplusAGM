using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gapplus.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedMeetingModelAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeetingDetails",
                table: "Meetings");

            migrationBuilder.AlterColumn<int>(
                name: "MeetingStatus",
                table: "Meetings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MeetingDetailsid",
                table: "Meetings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "MeetingDetails",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    uuid = table.Column<string>(type: "TEXT", nullable: false),
                    host_id = table.Column<string>(type: "TEXT", nullable: false),
                    host_email = table.Column<string>(type: "TEXT", nullable: false),
                    topic = table.Column<string>(type: "TEXT", nullable: false),
                    type = table.Column<int>(type: "INTEGER", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: false),
                    start_time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    duration = table.Column<int>(type: "INTEGER", nullable: false),
                    timezone = table.Column<string>(type: "TEXT", nullable: false),
                    agenda = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    start_url = table.Column<string>(type: "TEXT", nullable: false),
                    join_url = table.Column<string>(type: "TEXT", nullable: false),
                    password = table.Column<string>(type: "TEXT", nullable: false),
                    h323_password = table.Column<string>(type: "TEXT", nullable: false),
                    pstn_password = table.Column<string>(type: "TEXT", nullable: false),
                    encrypted_password = table.Column<string>(type: "TEXT", nullable: false),
                    pre_schedule = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingDetails", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_MeetingDetailsid",
                table: "Meetings",
                column: "MeetingDetailsid");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_MeetingDetails_MeetingDetailsid",
                table: "Meetings",
                column: "MeetingDetailsid",
                principalTable: "MeetingDetails",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_MeetingDetails_MeetingDetailsid",
                table: "Meetings");

            migrationBuilder.DropTable(
                name: "MeetingDetails");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_MeetingDetailsid",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "MeetingDetailsid",
                table: "Meetings");

            migrationBuilder.AlterColumn<string>(
                name: "MeetingStatus",
                table: "Meetings",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "MeetingDetails",
                table: "Meetings",
                type: "TEXT",
                nullable: true);
        }
    }
}
