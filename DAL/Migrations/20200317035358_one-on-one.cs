using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class oneonone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cv",
                table: "Cv");

            migrationBuilder.DropIndex(
                name: "IX_Cv_UserId",
                table: "Cv");

            migrationBuilder.DropColumn(
                name: "KeyName",
                table: "Cv");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cv",
                table: "Cv",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cv",
                table: "Cv");

            migrationBuilder.AddColumn<Guid>(
                name: "KeyName",
                table: "Cv",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cv",
                table: "Cv",
                column: "KeyName");

            migrationBuilder.CreateIndex(
                name: "IX_Cv_UserId",
                table: "Cv",
                column: "UserId",
                unique: true);
        }
    }
}
