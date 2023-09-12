using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandsomePattern.Migrations
{
    /// <inheritdoc />
    public partial class fk_dependencytypeid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "DependencyType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_File_DependencyTypeId",
                table: "File",
                column: "DependencyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_DependencyType_DependencyTypeId",
                table: "File",
                column: "DependencyTypeId",
                principalTable: "DependencyType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_DependencyType_DependencyTypeId",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_File_DependencyTypeId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "DependencyType");
        }
    }
}
