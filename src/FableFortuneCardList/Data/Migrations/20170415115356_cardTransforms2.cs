using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FableFortuneCardList.Data.Migrations
{
    public partial class cardTransforms2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CardID",
                table: "Card",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Card_CardID",
                table: "Card",
                column: "CardID");

            migrationBuilder.AddForeignKey(
                name: "FK_Card_Card_CardID",
                table: "Card",
                column: "CardID",
                principalTable: "Card",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Card_Card_CardID",
                table: "Card");

            migrationBuilder.DropIndex(
                name: "IX_Card_CardID",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "CardID",
                table: "Card");
        }
    }
}
