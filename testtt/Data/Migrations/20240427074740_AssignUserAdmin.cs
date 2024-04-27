using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testtt.Data.Migrations
{
    public partial class AssignUserAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[UserRoles] (UserId,RoleId) SELECT '432a6ab1-7795-4d4f-98af-ef494c83bebe',Id FROM [security].[Roles]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[UserRoles] WHERE UserId='432a6ab1-7795-4d4f-98af-ef494c83bebe'");
        }
    }
}
