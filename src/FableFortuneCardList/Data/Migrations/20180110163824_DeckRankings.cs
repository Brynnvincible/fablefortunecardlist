using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FableFortuneCardList.Data.Migrations
{
    public partial class DeckRankings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeckRanking",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeckID = table.Column<int>(nullable: false),
                    Ranking = table.Column<int>(nullable: false),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckRanking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeckRanking_Deck_DeckID",
                        column: x => x.DeckID,
                        principalTable: "Deck",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeckRanking_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeckRanking_DeckID",
                table: "DeckRanking",
                column: "DeckID");

            migrationBuilder.CreateIndex(
                name: "IX_DeckRanking_UserID",
                table: "DeckRanking",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeckRanking");
        }
    }
}
