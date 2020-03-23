using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DAL.Migrations
{
    public partial class Seed_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var pos1 = Guid.NewGuid();
            var pos2 = Guid.NewGuid();
            var pos3 = Guid.NewGuid();
            migrationBuilder.Sql("INSERT INTO public.\"Positions\"(\"PositionId\", \"Name\") VALUES('" + pos1 + "', 'junior')");
            migrationBuilder.Sql("INSERT INTO public.\"Positions\"(\"PositionId\", \"Name\") VALUES('" + pos2 + "', 'mid-level')");
            migrationBuilder.Sql("INSERT INTO public.\"Positions\"(\"PositionId\", \"Name\") VALUES('" + pos3 + "', 'senior')");

            migrationBuilder.Sql("INSERT INTO public.\"Challenges\"(\"ChallengeId\", \"Name\", \"Content\", \"PositionId\") VALUES('" + Guid.NewGuid() + "', 'challenge1', 'something1', '" + pos1 + "')");
            migrationBuilder.Sql("INSERT INTO public.\"Challenges\"(\"ChallengeId\", \"Name\", \"Content\", \"PositionId\") VALUES('" + Guid.NewGuid() + "', 'challenge2', 'something2', '" + pos1 + "')");
            migrationBuilder.Sql("INSERT INTO public.\"Challenges\"(\"ChallengeId\", \"Name\", \"Content\", \"PositionId\") VALUES('" + Guid.NewGuid() + "', 'challenge3', 'something3', '" + pos2 + "')");
            migrationBuilder.Sql("INSERT INTO public.\"Challenges\"(\"ChallengeId\", \"Name\", \"Content\", \"PositionId\") VALUES('" + Guid.NewGuid() + "', 'challenge4', 'something4', '" + pos2 + "')");
            migrationBuilder.Sql("INSERT INTO public.\"Challenges\"(\"ChallengeId\", \"Name\", \"Content\", \"PositionId\") VALUES('" + Guid.NewGuid() + "', 'challenge5', 'something5', '" + pos3 + "')");
            migrationBuilder.Sql("INSERT INTO public.\"Challenges\"(\"ChallengeId\", \"Name\", \"Content\", \"PositionId\") VALUES('" + Guid.NewGuid() + "', 'challenge6', 'something6', '" + pos3 + "')");
            migrationBuilder.Sql("INSERT INTO public.\"Challenges\"(\"ChallengeId\", \"Name\", \"Content\", \"PositionId\") VALUES('" + Guid.NewGuid() + "', 'Recursion', 'Do something cool with recursion. Cal f(5)=x + 5', '" + pos1 + "')");

            migrationBuilder.Sql("INSERT INTO public.\"Users\"(\"UserId\", \"Email\", \"Phone\", \"FullName\", \"ConfirmationCode\", \"PositionId\", \"DateCreate\") VALUES('" + Guid.NewGuid() + "', 'chon@gmail.com', '0888129182', 'Sir Chon', '9PB7CYEH8ZVcsFTDLmAJNgxrUzlObidy', '" + pos1 + "', '" + DateTime.Now + "'); ");
            migrationBuilder.Sql("INSERT INTO public.\"Users\"(\"UserId\", \"Email\", \"Phone\", \"FullName\", \"ConfirmationCode\", \"PositionId\", \"DateCreate\") VALUES('" + Guid.NewGuid() + "', 'wang@gmail.com', '34124263', 'Sir Wang', 'P03IytTDwNqsFdZVJBUK1v8GaizCYuHW', '" + pos1 + "', '" + (DateTime.Now.AddDays(7)) + "'); ");
            migrationBuilder.Sql("INSERT INTO public.\"Users\"(\"UserId\", \"Email\", \"Phone\", \"FullName\", \"ConfirmationCode\", \"PositionId\", \"DateCreate\") VALUES('" + Guid.NewGuid() + "', 'alibaba@gmail.com', '546543525432', 'Arabic Man', 'ZuMdKs8wfrImlCpnBJ26a3DizGtN4jYF', '" + pos2 + "', '" + (DateTime.Now.AddDays(2)) + "'); ");
            migrationBuilder.Sql("INSERT INTO public.\"Users\"(\"UserId\", \"Email\", \"Phone\", \"FullName\", \"ConfirmationCode\", \"PositionId\", \"DateCreate\") VALUES('" + Guid.NewGuid() + "', 'merde@gmail.com', '4143635634', 'French Weirdo', 'sD0g6YaJyCSc1muOLiMkoWAl3j4wTnXV', '" + pos2 + "', '" + (DateTime.Now.AddDays(4)) + "'); ");
            migrationBuilder.Sql("INSERT INTO public.\"Users\"(\"UserId\", \"Email\", \"Phone\", \"FullName\", \"ConfirmationCode\", \"PositionId\", \"DateCreate\") VALUES('" + Guid.NewGuid() + "', 'johndoe@gmail.com', '0888129182', 'John Doe', 'cGIYSJZu7mkrjDnORpgbWvTtwV8d6Lxq', '" + pos3 + "', '" + (DateTime.Now.AddDays(3)) + "'); ");
            migrationBuilder.Sql("INSERT INTO public.\"Users\"(\"UserId\", \"Email\", \"Phone\", \"FullName\", \"ConfirmationCode\", \"PositionId\", \"DateCreate\") VALUES('" + Guid.NewGuid() + "', 'janedoe@gmail.com', '5961959294', 'Jane Doe', 'jMgprd8qL4xsz92iZoeU1QHYOnJaAGKN', '" + pos3 + "', '" + (DateTime.Now.AddDays(5)) + "'); ");
                
            var adminId = Guid.NewGuid();
            migrationBuilder.Sql("INSERT INTO public.\"Positions\"(\"PositionId\", \"Name\") VALUES('" + adminId + "', 'admin')");
            migrationBuilder.Sql("INSERT INTO public.\"Users\"(\"UserId\", \"Email\", \"Phone\", \"FullName\", \"ConfirmationCode\", \"PositionId\", \"DateCreate\")" +
                " VALUES('" + Guid.NewGuid() + "', 'admin@admin.admin', '0888888888', 'Super Admin', '123456789','" + adminId + "', '" + DateTime.MinValue + "'); ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
