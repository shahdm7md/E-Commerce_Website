using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testtt.Data.Migrations
{
    public partial class AssignAdminUser3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[UserRoles] (UserId,RoleId) SELECT '30378682-1c37-4ca2-b487-7ad9ff42ef69',Id FROM [security].[Roles]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[UserRoles] WHERE UserId='30378682-1c37-4ca2-b487-7ad9ff42ef69'");
        }
    }
}
