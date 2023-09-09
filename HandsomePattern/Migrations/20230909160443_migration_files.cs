using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandsomePattern.Migrations
{
    /// <inheritdoc />
    public partial class migration_files : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    FileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Filename = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PathsToFile = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false),
                    Template = table.Column<string>(type: "nvarchar(MAX)", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.FileId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "File");
        }
    }
}
