using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorWebForum.Migrations
{
    /// <inheritdoc />
    public partial class F1_Add_IsDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Post",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Feedback",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "AspNetUsers");
        }
    }
}
