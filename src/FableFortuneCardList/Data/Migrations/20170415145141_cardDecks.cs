using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FableFortuneCardList.Data.Migrations
{
    public partial class cardDecks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deck",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deck", x => x.ID);
                });

            migrationBuilder.AddColumn<string>(
                name: "DeckID",
                table: "Card",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Card_DeckID",
                table: "Card",
                column: "DeckID");

            migrationBuilder.AddForeignKey(
                name: "FK_Card_Deck_DeckID",
                table: "Card",
                column: "DeckID",
                principalTable: "Deck",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Card_Deck_DeckID",
                table: "Card");

            migrationBuilder.DropIndex(
                name: "IX_Card_DeckID",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "DeckID",
                table: "Card");

            migrationBuilder.DropTable(
                name: "Deck");
        }
    }
}
