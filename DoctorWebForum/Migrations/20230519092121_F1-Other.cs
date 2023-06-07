using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorWebForum.Migrations
{
    /// <inheritdoc />
    public partial class F1Other : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Experience",
                columns: new[] { "Id", "Years" },
                values: new object[] { 5, " Other " });

            migrationBuilder.InsertData(
                table: "Qualification",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, " Other" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Experience",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Qualification",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
