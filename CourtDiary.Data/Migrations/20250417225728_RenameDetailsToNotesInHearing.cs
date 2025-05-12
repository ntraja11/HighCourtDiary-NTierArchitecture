using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDiary.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameDetailsToNotesInHearing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Details",
                table: "Hearings",
                newName: "Notes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Hearings",
                newName: "Details");
        }
    }
}
