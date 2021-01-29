using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bank.Orchestrators.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransferStates",
                columns: table => new
                {
                    TransferId = table.Column<Guid>(nullable: false),
                    CurrentState = table.Column<string>(nullable: false),
                    SourceAccountId = table.Column<Guid>(nullable: false),
                    TargetAccountId = table.Column<Guid>(nullable: false),
                    Sum = table.Column<decimal>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferStates", x => x.TransferId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferStates");
        }
    }
}
