using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DAL.Migrations
{
    public partial class Add_Admin_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
