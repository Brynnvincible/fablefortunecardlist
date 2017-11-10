using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using FableFortuneCardList.Enums;

namespace FableFortuneCardList.Data.Migrations
{
    public partial class cards2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassID",
                table: "Card");

            migrationBuilder.AddColumn<int>(
                name: "Class",
                table: "Card",
                nullable: false,
                defaultValue: (int)ClassType.Neutral);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class",
                table: "Card");

            migrationBuilder.AddColumn<int>(
                name: "ClassID",
                table: "Card",
                nullable: false,
                defaultValue: 0);
        }
    }
}
