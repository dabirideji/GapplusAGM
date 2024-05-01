using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gapplus.Api.Migrations
{
    /// <inheritdoc />
    public partial class AlteredModelMeetingDetailddghvghcgfcddains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingDetails_Meetings_MeetingId",
                table: "MeetingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MeetingSettings_MeetingDetails_MeetingDetaildId",
                table: "MeetingSettings");

            migrationBuilder.DropIndex(
                name: "IX_MeetingSettings_MeetingDetaildId",
                table: "MeetingSettings");

            migrationBuilder.DropIndex(
                name: "IX_MeetingDetails_MeetingId",
                table: "MeetingDetails");

            migrationBuilder.DropColumn(
                name: "MeetingDetaildId",
                table: "MeetingSettings");

            migrationBuilder.DropColumn(
                name: "MeetingId",
                table: "MeetingDetails");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_MeetingDetailsId",
                table: "Meetings",
                column: "MeetingDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_MeetingDetails_MeetingDetailsId",
                table: "Meetings",
                column: "MeetingDetailsId",
                principalTable: "MeetingDetails",
                principalColumn: "MeetingDetailsId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_MeetingDetails_MeetingDetailsId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_MeetingDetailsId",
                table: "Meetings");

            migrationBuilder.AddColumn<Guid>(
                name: "MeetingDetaildId",
                table: "MeetingSettings",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MeetingId",
                table: "MeetingDetails",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MeetingSettings_MeetingDetaildId",
                table: "MeetingSettings",
                column: "MeetingDetaildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeetingDetails_MeetingId",
                table: "MeetingDetails",
                column: "MeetingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingDetails_Meetings_MeetingId",
                table: "MeetingDetails",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "MeetingId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingSettings_MeetingDetails_MeetingDetaildId",
                table: "MeetingSettings",
                column: "MeetingDetaildId",
                principalTable: "MeetingDetails",
                principalColumn: "MeetingDetailsId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
