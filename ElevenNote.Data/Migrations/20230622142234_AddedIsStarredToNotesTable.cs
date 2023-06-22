using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElevenNote.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsStarredToNotesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStarred",
                table: "Notes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStarred",
                table: "Notes");
        }
    }
}
