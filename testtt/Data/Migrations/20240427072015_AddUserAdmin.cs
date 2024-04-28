using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testtt.Data.Migrations
{
    public partial class AddUserAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[Users] ([Id], [Cus_FName], [Cus_LName], [Cus_address], [Cus_ImageUrl], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'432a6ab1-7795-4d4f-98af-ef494c83bebe', N'Rana', N'ali', N'assuit', NULL, N'roro56@yahoo.com', N'RORO56@YAHOO.COM', N'roro56@yahoo.com', N'RORO56@YAHOO.COM', 0, N'AQAAAAEAACcQAAAAEJNt7/4I7aHD0qKaAT/gXcy3IEm2ajqzV/fShImxb4fWlnnj8yMaEfIed2NjjTA/IA==', N'3XTXS55JER4X4QWFCRYKNGHHNPJIQRHE', N'056536b5-a9e6-45e4-b662-cc26c576c0ec', NULL, 0, 0, NULL, 1, 0)\r\n");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELET FROM [security].[Users] WHERE Id='432a6ab1-7795-4d4f-98af-ef494c83bebe'");
        }
    }
}
