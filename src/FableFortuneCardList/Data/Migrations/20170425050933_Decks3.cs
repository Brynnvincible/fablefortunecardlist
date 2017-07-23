using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FableFortuneCardList.Data.Migrations
{
    public partial class Decks3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Card_Card_CardID",
                table: "Card");

            migrationBuilder.DropForeignKey(
                name: "FK_Card_Deck_DeckID",
                table: "Card");

            migrationBuilder.DropIndex(
                name: "IX_Card_CardID",
                table: "Card");

            migrationBuilder.DropIndex(
                name: "IX_Card_DeckID",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "CardID",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "DeckID",
                table: "Card");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Deck",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Card",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Deck",
                nullable: false);

            migrationBuilder.DropPrimaryKey("PK_Deck", "Deck");
            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Deck",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
            migrationBuilder.AddPrimaryKey("PK_Deck", "Deck","ID");
            migrationBuilder.CreateIndex(
                name: "IX_Deck_CreatedById",
                table: "Deck",
                column: "CreatedById");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Card",
                nullable: false);

            migrationBuilder.CreateTable(
            name: "DeckCard",
            columns: table => new
            {
                CardId = table.Column<int>(nullable: false),
                DeckId = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DeckCard", x => new { x.CardId, x.DeckId });
                table.ForeignKey(
                    name: "FK_DeckCard_Card_CardId",
                    column: x => x.CardId,
                    principalTable: "Card",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_DeckCard_Deck_DeckId",
                    column: x => x.DeckId,
                    principalTable: "Deck",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Cascade);
            });

            migrationBuilder.CreateIndex(
                name: "IX_DeckCard_CardId",
                table: "DeckCard",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_DeckCard_DeckId",
                table: "DeckCard",
                column: "DeckId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deck_AspNetUsers_CreatedById",
                table: "Deck",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Deck",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deck_AspNetUsers_CreatedById",
                table: "Deck");

            migrationBuilder.DropIndex(
                name: "IX_Deck_CreatedById",
                table: "Deck");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Deck");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Card");

            migrationBuilder.DropTable(
                name: "DeckCard");

            migrationBuilder.AddColumn<int>(
                name: "CardID",
                table: "Card",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeckID",
                table: "Card",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Deck",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Deck",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Card",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Card_CardID",
                table: "Card",
                column: "CardID");

            migrationBuilder.CreateIndex(
                name: "IX_Card_DeckID",
                table: "Card",
                column: "DeckID");

            migrationBuilder.AddForeignKey(
                name: "FK_Card_Card_CardID",
                table: "Card",
                column: "CardID",
                principalTable: "Card",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Card_Deck_DeckID",
                table: "Card",
                column: "DeckID",
                principalTable: "Deck",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Deck",
                newName: "ID");
        }
    }
}
