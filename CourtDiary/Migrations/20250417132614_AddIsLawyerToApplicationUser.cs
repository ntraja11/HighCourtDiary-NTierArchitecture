using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDiary.Migrations
{
    /// <inheritdoc />
    public partial class AddIsLawyerToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLawyer",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLawyer",
                table: "AspNetUsers");
        }
    }
}
