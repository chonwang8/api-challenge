using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DAL.Migrations
{
    public partial class SeedChallenges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO public.\"Challenges\"(\"ChallengeId\", \"Name\", \"Content\", \"PositionId\") VALUES('" + Guid.NewGuid() + "', 'Recursion', 'Do something cool with recursion. Cal f(5)=x + 5', 'fc6b57bf-2532-450d-80b8-113f0fb97335')");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
