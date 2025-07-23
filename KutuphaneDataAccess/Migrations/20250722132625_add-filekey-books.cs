using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KutuphaneDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addfilekeybooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileKey",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileKey",
                table: "Books");
        }
    }
}
