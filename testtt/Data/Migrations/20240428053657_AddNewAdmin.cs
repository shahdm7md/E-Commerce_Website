using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testtt.Data.Migrations
{
    public partial class AddNewAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[Users] ([Id], [Cus_FName], [Cus_LName], [Cus_address], [Cus_ImageUrl], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'41e94513-4251-42a7-af38-c5aba74583a1', N'radwa', N'mostafa', N'assuit', NULL, N'radwa56taher@gmail.com', N'RADWA56TAHER@GMAIL.COM', N'radwa56taher@gmail.com', N'RADWA56TAHER@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEONZCz5IWJTqWj5Mvi8r5PpI5+dNGi619eNwOwDKPq/kuhwb4E8UVbUPtIKa+kAIYg==', N'TQN7KXLPEKVCOGJ7RECPBPK4BUA46XG6', N'a12673c8-160f-43d0-8b1a-b6adf4a57c59', NULL, 0, 0, NULL, 1, 0)\r\n");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[Users] WHERE Id='41e94513-4251-42a7-af38-c5aba74583a1'");
        }
    }
}
