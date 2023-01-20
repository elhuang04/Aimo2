using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aimo2.Data.Migrations
{
    /// <inheritdoc />
    public partial class addexplore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Explore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PeopleNeeded = table.Column<int>(name: "People_Needed", type: "INTEGER", nullable: false),
                    Requester = table.Column<string>(type: "TEXT", nullable: false),
                    AcceptedBy = table.Column<string>(name: "Task_Title", type: "TEXT", nullable: false),
                    DueDate = table.Column<DateTime>(name: "Due_Date", type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    People_Claimed = table.Column<int>(type: "INTEGER", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Explore", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Explore");
        }
    }
}
