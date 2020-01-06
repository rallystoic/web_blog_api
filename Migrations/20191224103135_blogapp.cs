using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace finalBlog.Migrations
{
    public partial class blogapp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    categoryid = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    categoryname = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.categoryid);
                });

            migrationBuilder.CreateTable(
                name: "contents",
                columns: table => new
                {
                    contentid = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    contentdetail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contents", x => x.contentid);
                });

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    postid = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    posttitle = table.Column<string>(nullable: true),
                    titleimgurl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posts", x => x.postid);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    roleid = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    rolename = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.roleid);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userid = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(nullable: true),
                    usersalt = table.Column<string>(nullable: true),
                    userhashed = table.Column<string>(nullable: true),
                    userfirstname = table.Column<string>(nullable: true),
                    userlastname = table.Column<string>(nullable: true),
                    roleid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.userid);
                    table.ForeignKey(
                        name: "FK_users_roles_roleid",
                        column: x => x.roleid,
                        principalTable: "roles",
                        principalColumn: "roleid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "postcontents",
                columns: table => new
                {
                    postcontentid = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    postid = table.Column<int>(nullable: false),
                    contentid = table.Column<int>(nullable: false),
                    userid = table.Column<int>(nullable: false),
                    categoryid = table.Column<int>(nullable: false),
                    postcontentcreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_postcontents", x => x.postcontentid);
                    table.ForeignKey(
                        name: "FK_postcontents_categories_categoryid",
                        column: x => x.categoryid,
                        principalTable: "categories",
                        principalColumn: "categoryid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_postcontents_contents_contentid",
                        column: x => x.contentid,
                        principalTable: "contents",
                        principalColumn: "contentid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_postcontents_posts_postid",
                        column: x => x.postid,
                        principalTable: "posts",
                        principalColumn: "postid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_postcontents_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "highlights",
                columns: table => new
                {
                    highlightid = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    postcontentid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_highlights", x => x.highlightid);
                    table.ForeignKey(
                        name: "FK_highlights_postcontents_postcontentid",
                        column: x => x.postcontentid,
                        principalTable: "postcontents",
                        principalColumn: "postcontentid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_highlights_postcontentid",
                table: "highlights",
                column: "postcontentid");

            migrationBuilder.CreateIndex(
                name: "IX_postcontents_categoryid",
                table: "postcontents",
                column: "categoryid");

            migrationBuilder.CreateIndex(
                name: "IX_postcontents_contentid",
                table: "postcontents",
                column: "contentid");

            migrationBuilder.CreateIndex(
                name: "IX_postcontents_postid",
                table: "postcontents",
                column: "postid");

            migrationBuilder.CreateIndex(
                name: "IX_postcontents_userid",
                table: "postcontents",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_users_roleid",
                table: "users",
                column: "roleid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "highlights");

            migrationBuilder.DropTable(
                name: "postcontents");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "contents");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
