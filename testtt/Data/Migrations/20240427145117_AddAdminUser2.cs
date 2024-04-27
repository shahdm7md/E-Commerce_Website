using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;
using System.Security;

#nullable disable

namespace testtt.Data.Migrations
{
    public partial class AddAdminUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO[security].[Users] ([Id], [Cus_FName], [Cus_LName], [Cus_address], [Cus_ImageUrl], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES(N'1dfd4c29-48c9-4540-90ea-bd6e97632669', N'admhas', N'khal', N'assiut', NULL, N'admin@test.com', N'ADMIN@TEST.COM', N'admin@test.com', N'ADMIN@TEST.COM', 0, N'AQAAAAEAACcQAAAAEMDPGz7v/wYKM//RPUlSVp/D9N4pO3EjSTc7enpQOCDL4FwAUQDSd/bBqTQiKCvFfw==', N'JQGTVWCNSJQMUTTEM7QU5ISX7VYZLUCU', N'30ae2800-289d-48c0-96e8-85fcdecd0a3a', NULL, 0, 0, NULL, 1, 0)\r\n");

        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELET FROM [security].[Users] WHERE Id='1dfd4c29-48c9-4540-90ea-bd6e97632669'");
        }
    }
}
