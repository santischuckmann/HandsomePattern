using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandsomePattern.Migrations
{
    /// <inheritdoc />
    public partial class insercion_de_templates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @$"
                    SET IDENTITY_INSERT [dbo].[File] ON;
                  
                    INSERT INTO [dbo].[File](FileId, Filename, PathsToFile, Template, DependencyTypeId) Values 
                        (1, '[[currentNamespace]]DbContext.cs',  'Data', '{Templates.DbContextTemplate}', 0),
                        (2, 'ServiceCollectionExtensions.cs', 'Extensions', '{Templates.ServiceExtensionsTemplate}', 0),
                        (3, 'BaseRepository.cs', 'Repositories', '{Templates.BaseRepositoryTemplate}', 0),
                        (4, 'UnitOfWork.cs', 'Repositories', '{Templates.UnitOfWorkTemplate}', 0),
                        (5, 'IRepository.cs', 'Interfaces//Repositories', '{Templates.IRepositoryTemplate}', 0),
                        (6, 'IUnitOfWork.cs', 'Interfaces//Repositories', '{Templates.IUnitOfWorkTemplate}', 0)

                    SET IDENTITY_INSERT [dbo].[File] OFF";

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
