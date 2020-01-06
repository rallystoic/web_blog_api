using Microsoft.EntityFrameworkCore.Migrations;

namespace finalBlog.Migrations
{
    public partial class finalblog02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "roleid", "rolename" },
                values: new object[] { 1, "administrator" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "roleid", "rolename" },
                values: new object[] { 2, "author" });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "userid", "roleid", "userfirstname", "userhashed", "userlastname", "username", "usersalt" },
                values: new object[] { 1, 1, "admin", "9DDbStYYrn8ExReQN62o5J3wpVZUztBVMoEFqW3Ko2A=", "admin", "admin", "cztlN43JJ1nDn1NWuvAnpA==" });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "userid", "roleid", "userfirstname", "userhashed", "userlastname", "username", "usersalt" },
                values: new object[] { 2, 2, "koala", "CTXXZzNjBghOuo1X2353N7VWJNysSVwcXR2+VsiZwuA=", "koalo", "koala02", "vuc7ESG50L8bY9gUjvx9HQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "userid",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "userid",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "roleid",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "roleid",
                keyValue: 2);
        }
    }
}
