using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testtt.Data.Migrations
{
    public partial class AddAdminUser3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[Users] ([Id], [Cus_FName], [Cus_LName], [Cus_address], [Cus_ImageUrl], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'30378682-1c37-4ca2-b487-7ad9ff42ef69', N'testa', N'admin', N'assiut', NULL, N'testadmin@test.com', N'TESTADMIN@TEST.COM', N'testadmin@test.com', N'TESTADMIN@TEST.COM', 1, N'AQAAAAEAACcQAAAAEEsh8PccaBzG14X8ebR/a9Gbh/3jGi/9NUg2rWohKAZkRp+6VwAtALiIXoP9TCQ+qA==', N'FFHNGZNLR27QM7XHEDHYSX5JK5O4OU3H', N'8f323e1e-b2bd-4375-b7bf-e78772882d70', NULL, 0, 0, NULL, 1, 0)\r\n");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELET FROM [security].[Users] WHERE Id='30378682-1c37-4ca2-b487-7ad9ff42ef69'");
        }
    }
}
