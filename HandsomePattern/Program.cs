using HandsomePattern.Logic;

namespace HandsomePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            // should be read from a configuration file, or from input interactively
            string globalNamespace = args[0];

            string rootDirectory = Directory.GetCurrentDirectory();

            if (rootDirectory.Contains("Release") || rootDirectory.Contains("Debug"))
            {
                rootDirectory = rootDirectory.Split($"\\{globalNamespace}\\bin")[0];
            }

            bool isSolution = Directory.GetFiles(rootDirectory, "*.sln").Length > 0;

            if (!isSolution) throw new Exception("Para utilizar esta herramienta, posicionese dentro de una solucion de C#.");

            string[] packages = new string[] { "Microsoft.EntityFrameworkCore", "Microsoft.EntityFrameworkCore.SqlServer", "Microsoft.OpenApi", "AutoMapper.Extensions.Microsoft.DependencyInjection", "Swashbuckle.AspNetCore.SwaggerGen" };

            ProjectProperties infrastructureProperties = new ProjectProperties(projectName: "Infrastructure", globalNamespace: globalNamespace, rootDirectory: rootDirectory);

            Project infrastructure = new Project(infrastructureProperties, packages, new string[] { "..\\[[currentNamespace]].Core\\[[currentNamespace]].Core.csproj" });

            infrastructure.DoesProjectExist(rootDirectory);
            infrastructure.CorrectProjectReferences();
            infrastructure.CheckDependencies();

            List<FileCreationArgs> infraFileCreationsArgs = new List<FileCreationArgs>
            {
                new FileCreationArgs()
                {
                    Filename = "[[currentNamespace]]DbContext.cs",
                    PathsToFile = new string[] { "Data" },
                    Template = Templates.DbContextTemplate
                },

                new FileCreationArgs()
                {
                    Filename = "ServiceCollectionExtensions.cs",
                    PathsToFile = new string[] { "Extensions" },
                    Template = Templates.ServiceExtensionsTemplate
                },
                
                new FileCreationArgs()
                {
                    Filename = "BaseRepository.cs",
                    PathsToFile = new string[] { "Repositories" },
                    Template = Templates.BaseRepositoryTemplate
                },

                new FileCreationArgs()
                {
                    Filename = "UnitOfWork.cs",
                    PathsToFile = new string[] { "Repositories" },
                    Template = Templates.UnitOfWorkTemplate,
                    DependencyType = Enums.DependencyType.Repository
                }
            };

            ProjectProperties coreProperties = new ProjectProperties(projectName: "Core", globalNamespace: globalNamespace, rootDirectory: rootDirectory);

            Project core = new Project(coreProperties, new string[] { }, new string[] { });

            core.DoesProjectExist(rootDirectory);
            core.CorrectProjectReferences();

            List<FileCreationArgs> coreFileCreationsArgs = new List<FileCreationArgs>
            {
                new FileCreationArgs()
                {
                    Filename = "IRepository.cs",
                    PathsToFile = new string[] { "Interfaces", "Repositories" },
                    Template = Templates.IRepositoryTemplate,
                },

                new FileCreationArgs()
                {
                    Filename = "IUnitOfWork.cs",
                    PathsToFile = new string[] { "Interfaces", "Repositories" },
                    Template = Templates.IUnitOfWorkTemplate,
                },

                new FileCreationArgs()
                {
                    Filename = "CommonEntity.cs",
                    PathsToFile = new string[] { "Entitys" },
                    Template = Templates.CommonEntityTemplate,
                },
            };

            CreationExecution infraProcess = new CreationExecution(infrastructureProperties, infraFileCreationsArgs);

            infraProcess.Execute();

            CreationExecution coreProcess = new CreationExecution(coreProperties, coreFileCreationsArgs);

            coreProcess.Execute();
        }
    }
}

