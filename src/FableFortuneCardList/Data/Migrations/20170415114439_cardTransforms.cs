using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FableFortuneCardList.Data.Migrations
{
    public partial class cardTransforms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Transform",
                table: "Card",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransformType",
                table: "Card",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Transform",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "TransformType",
                table: "Card");
        }
    }
}
