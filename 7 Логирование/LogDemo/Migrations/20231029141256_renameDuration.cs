using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogDemo.Migrations
{
    /// <inheritdoc />
    public partial class renameDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DurationInSecond",
                table: "Tracks",
                newName: "DurationInSeconds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DurationInSeconds",
                table: "Tracks",
                newName: "DurationInSecond");
        }
    }
}
