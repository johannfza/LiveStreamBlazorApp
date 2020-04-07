using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaManager.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LiveStream",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    DatePublished = table.Column<string>(nullable: true),
                    Views = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveStream", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "LiveStream",
                columns: new[] { "Id", "DatePublished", "Description", "Title", "Url", "Views" },
                values: new object[,]
                {
                    { 1, "4/7/2020 5:08:17 PM", "This is Live Stream 1", "LiveStream 1", "http://www.golive.com/1", 0 },
                    { 2, "4/7/2020 5:08:17 PM", "This is Live Stream 2", "LiveStream 2", "http://www.golive.com/2", 0 },
                    { 3, "4/7/2020 5:08:17 PM", "This is Live Stream 3", "LiveStream 3", "http://www.golive.com/3", 0 },
                    { 4, "4/7/2020 5:08:17 PM", "This is Live Stream 4", "LiveStream 4", "http://www.golive.com/4", 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiveStream");
        }
    }
}
