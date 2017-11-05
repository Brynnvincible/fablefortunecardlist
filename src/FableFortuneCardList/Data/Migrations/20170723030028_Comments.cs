using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FableFortuneCardList.Data.Migrations
{
    public partial class Comments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentThread",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentThread", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    DownVotes = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    RepliedToID = table.Column<int>(nullable: true),
                    ThreadID = table.Column<int>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    UpVotes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comment_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comment_Comment_RepliedToID",
                        column: x => x.RepliedToID,
                        principalTable: "Comment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comment_CommentThread_ThreadID",
                        column: x => x.ThreadID,
                        principalTable: "CommentThread",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CreatedById",
                table: "Comment",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_RepliedToID",
                table: "Comment",
                column: "RepliedToID");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ThreadID",
                table: "Comment",
                column: "ThreadID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "CommentThread");
        }
    }
}
