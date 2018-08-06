using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnnouncementsAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "announcement",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    ImageName = table.Column<string>(nullable: true),
                    VipAnnouncement = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_announcement", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "announcement");
        }
    }
}
