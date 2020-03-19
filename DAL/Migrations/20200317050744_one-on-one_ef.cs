using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class oneonone_ef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cv_Users_UserId",
                table: "Cv");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cv",
                table: "Cv");

            migrationBuilder.RenameTable(
                name: "Cv",
                newName: "Cvs");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Cvs_Users_UserId",
                table: "Cvs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cvs_Users_UserId",
                table: "Cvs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cvs",
                table: "Cvs");

            migrationBuilder.DropIndex(
                name: "IX_Cvs_UserId",
                table: "Cvs");

            migrationBuilder.DropColumn(
                name: "CvId",
                table: "Cvs");

            migrationBuilder.RenameTable(
                name: "Cvs",
                newName: "Cv");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cv",
                table: "Cv",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cv_Users_UserId",
                table: "Cv",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
