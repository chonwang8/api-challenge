using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DAL.Migrations
{
    public partial class Addchallenges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO public.\"Challenges\"(\"ChallengeId\", \"Name\", \"Content\", \"PositionId\") VALUES('" + Guid.NewGuid() + "', 'challenge1', 'something1', 'fc6b57bf-2532-450d-80b8-113f0fb97335')");
            migrationBuilder.Sql("INSERT INTO public.\"Challenges\"(\"ChallengeId\", \"Name\", \"Content\", \"PositionId\") VALUES('" + Guid.NewGuid() + "', 'challenge2', 'something2', 'fc6b57bf-2532-450d-80b8-113f0fb97335')");
            migrationBuilder.Sql("INSERT INTO public.\"Challenges\"(\"ChallengeId\", \"Name\", \"Content\", \"PositionId\") VALUES('" + Guid.NewGuid() + "', 'challenge3', 'something3', 'fc6b57bf-2532-450d-80b8-113f0fb97335')");
            migrationBuilder.Sql("INSERT INTO public.\"Challenges\"(\"ChallengeId\", \"Name\", \"Content\", \"PositionId\") VALUES('" + Guid.NewGuid() + "', 'challenge4', 'something4', 'e4179214-4bf9-481e-b186-5a08d95f0b71')");
            migrationBuilder.Sql("INSERT INTO public.\"Challenges\"(\"ChallengeId\", \"Name\", \"Content\", \"PositionId\") VALUES('" + Guid.NewGuid() + "', 'challenge5', 'something5', 'e4179214-4bf9-481e-b186-5a08d95f0b71')");
            migrationBuilder.Sql("INSERT INTO public.\"Challenges\"(\"ChallengeId\", \"Name\", \"Content\", \"PositionId\") VALUES('" + Guid.NewGuid() + "', 'challenge6', 'something6', 'ab1a22fd-f614-4769-932f-1b62ee630f89')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
