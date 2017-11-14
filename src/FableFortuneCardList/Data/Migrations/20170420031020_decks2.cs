using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using FableFortuneCardList.Enums;

namespace FableFortuneCardList.Data.Migrations
{
    public partial class decks2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Class",
                table: "Deck",
                nullable: false,
                defaultValue: (int)ClassType.Neutral);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Card",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class",
                table: "Deck");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Card");
        }
    }
}
