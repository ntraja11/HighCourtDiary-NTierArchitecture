using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourtDiary.Data.Migrations
{
    /// <inheritdoc />
    public partial class DateFieldsAddedToOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "ActivatedDate",
                table: "Organizations",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedDate",
                table: "Organizations",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivatedDate",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Organizations");
        }
    }
}
