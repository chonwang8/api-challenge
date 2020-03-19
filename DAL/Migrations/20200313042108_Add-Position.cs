using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DAL.Migrations
{
    public partial class AddPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO public.\"Positions\"(\"PositionId\", \"Name\") VALUES('" + Guid.NewGuid() + "', 'junior')");
            migrationBuilder.Sql("INSERT INTO public.\"Positions\"(\"PositionId\", \"Name\") VALUES('" + Guid.NewGuid() + "', 'mid-level')");
            migrationBuilder.Sql("INSERT INTO public.\"Positions\"(\"PositionId\", \"Name\") VALUES('" + Guid.NewGuid() + "', 'senior')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
