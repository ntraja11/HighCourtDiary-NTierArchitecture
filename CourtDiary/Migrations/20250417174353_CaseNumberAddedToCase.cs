using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDiary.Migrations
{
    /// <inheritdoc />
    public partial class CaseNumberAddedToCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CaseNumber",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaseNumber",
                table: "Cases");
        }
    }
}
