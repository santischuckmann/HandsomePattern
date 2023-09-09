using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandsomePattern.Migrations
{
    /// <inheritdoc />
    public partial class template_type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Template",
                table: "File",
                type: "nvarchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false);

            migrationBuilder.AddColumn<int>(
                name: "DependencyTypeId",
                table: "File",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DependencyTypeId",
                table: "File");

            migrationBuilder.AlterColumn<string>(
                name: "Template",
                table: "File",
                type: "varchar(max)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)",
                oldUnicode: false);
        }
    }
}
