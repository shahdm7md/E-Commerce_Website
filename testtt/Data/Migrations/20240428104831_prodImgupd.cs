using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testtt.Data.Migrations
{
    public partial class prodImgupd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prod_ImageUrl",
                table: "Products");

            migrationBuilder.AddColumn<byte[]>(
                name: "Prod_Image",
                table: "Products",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prod_Image",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Prod_ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
