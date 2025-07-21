using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Blog.Migrations
{
    /// <inheritdoc />
    public partial class Authentication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "content",
                table: "Blogs",
                newName: "Content");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Blogs",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Blogs",
                newName: "content");
        }
    }
}
