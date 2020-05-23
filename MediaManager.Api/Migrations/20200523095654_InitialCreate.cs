using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaManager.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    ConnectionId = table.Column<string>(nullable: false),
                    RoomName = table.Column<string>(nullable: true),
                    Connected = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.ConnectionId);
                });

            migrationBuilder.CreateTable(
                name: "LiveStream",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false),
                    StreamKey = table.Column<string>(nullable: false),
                    DatePublished = table.Column<string>(nullable: true),
                    Views = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveStream", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserConnections",
                columns: table => new
                {
                    ConnectionId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConnections", x => new { x.UserId, x.ConnectionId });
                    table.ForeignKey(
                        name: "FK_UserConnections_Connections_ConnectionId",
                        column: x => x.ConnectionId,
                        principalTable: "Connections",
                        principalColumn: "ConnectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserConnections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "LiveStream",
                columns: new[] { "Id", "DatePublished", "Description", "StreamKey", "Title", "Url", "Views" },
                values: new object[,]
                {
                    { 1, "23/05/2020 17:56:50", "This is Live Stream 1", "key1", "LiveStream 1", "http://www.golive.com/1", 0 },
                    { 2, "23/05/2020 17:56:50", "This is Live Stream 2", "key2", "LiveStream 2", "http://www.golive.com/2", 0 },
                    { 3, "23/05/2020 17:56:50", "This is Live Stream 3", "key3", "LiveStream 3", "http://www.golive.com/3", 0 },
                    { 4, "23/05/2020 17:56:50", "This is Live Stream 4", "key4", "LiveStream 4", "http://www.golive.com/4", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserConnections_ConnectionId",
                table: "UserConnections",
                column: "ConnectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiveStream");

            migrationBuilder.DropTable(
                name: "UserConnections");

            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
