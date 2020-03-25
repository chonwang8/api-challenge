using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class addCVID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cvs",
                table: "Cvs");

            migrationBuilder.AddColumn<Guid>(
                name: "CvId",
                table: "Cvs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cvs",
                table: "Cvs",
                column: "CvId");

            migrationBuilder.CreateIndex(
                name: "IX_Cvs_UserId",
                table: "Cvs",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cvs",
                table: "Cvs");

            migrationBuilder.DropIndex(
                name: "IX_Cvs_UserId",
                table: "Cvs");

            migrationBuilder.DropColumn(
                name: "CvId",
                table: "Cvs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cvs",
                table: "Cvs",
                column: "UserId");
        }
    }
}
