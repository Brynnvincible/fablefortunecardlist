using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using FableFortuneCardList.Enums;

namespace FableFortuneCardList.Data.Migrations
{
    public partial class cards3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnitClass",
                table: "Card",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<int>(
                name: "SheetId",
                table: "Card",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitClass",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "SheetId",
                table: "Card");
        }
    }
}
