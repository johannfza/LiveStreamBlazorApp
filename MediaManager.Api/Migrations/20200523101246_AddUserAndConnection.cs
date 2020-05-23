using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaManager.Api.Migrations
{
    public partial class AddUserAndConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LiveStream",
                keyColumn: "Id",
                keyValue: 1,
                column: "DatePublished",
                value: "23/05/2020 18:12:45");

            migrationBuilder.UpdateData(
                table: "LiveStream",
                keyColumn: "Id",
                keyValue: 2,
                column: "DatePublished",
                value: "23/05/2020 18:12:45");

            migrationBuilder.UpdateData(
                table: "LiveStream",
                keyColumn: "Id",
                keyValue: 3,
                column: "DatePublished",
                value: "23/05/2020 18:12:45");

            migrationBuilder.UpdateData(
                table: "LiveStream",
                keyColumn: "Id",
                keyValue: 4,
                column: "DatePublished",
                value: "23/05/2020 18:12:45");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LiveStream",
                keyColumn: "Id",
                keyValue: 1,
                column: "DatePublished",
                value: "23/05/2020 17:56:50");

            migrationBuilder.UpdateData(
                table: "LiveStream",
                keyColumn: "Id",
                keyValue: 2,
                column: "DatePublished",
                value: "23/05/2020 17:56:50");

            migrationBuilder.UpdateData(
                table: "LiveStream",
                keyColumn: "Id",
                keyValue: 3,
                column: "DatePublished",
                value: "23/05/2020 17:56:50");

            migrationBuilder.UpdateData(
                table: "LiveStream",
                keyColumn: "Id",
                keyValue: 4,
                column: "DatePublished",
                value: "23/05/2020 17:56:50");
        }
    }
}
