using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FableFortuneCardList.Data.Migrations
{
    public partial class DeckCardFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DeckCard",
                table: "DeckCard");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DeckCard",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeckCard",
                table: "DeckCard",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DeckCard",
                table: "DeckCard");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DeckCard");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeckCard",
                table: "DeckCard",
                columns: new[] { "CardId", "DeckId" });
        }
    }
}
