using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testtt.Data.Migrations
{
    public partial class init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Prod_ImageUrl",
                schema: "security",
                table: "Users",
                newName: "Cus_ImageUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cus_ImageUrl",
                schema: "security",
                table: "Users",
                newName: "Prod_ImageUrl");
        }
    }
}
