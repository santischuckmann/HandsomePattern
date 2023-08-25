namespace HandsomePattern
{
    class Program
    {
        static void Main(string[] args)
        {

            string globalNamespace =  typeof(Program).Namespace;

            string rootDirectory = Directory.GetCurrentDirectory();

            if (rootDirectory.Contains("Release") || rootDirectory.Contains("Debug"))
            {
                rootDirectory = rootDirectory.Split($"\\{globalNamespace}\\bin")[0];
            }

            bool isSolution = Directory.GetFiles(rootDirectory, "*.sln").Length > 0;

            if (!isSolution) throw new Exception("Para utilizar esta herramienta, posicionese dentro de una solucion de C#.");

            string[] packages = new string[] { "Microsoft.EntityFrameworkCore", "Microsoft.EntityFrameworkCore.SqlServer", "Microsoft.OpenApi", "AutoMapper.Extensions.Microsoft.DependencyInjection", "Swashbuckle.AspNetCore.SwaggerGen" };

            ProjectProperties infrastructureProperties = new ProjectProperties(projectName: "Infrastructure", globalNamespace: globalNamespace, rootDirectory: rootDirectory);

            List<FileCreationArgs> infraFileCreationsArgs = new List<FileCreationArgs>
            {
                new FileCreationArgs()
                {
                    Filename = "[[currentNamespace]]DbContext.cs",
                    PathsToFile = new string[] { "Data" },
                    Template = Templates.UnitOfWorkTemplate
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
                }
            };

            ProjectProperties coreProperties = new ProjectProperties(projectName: "Core", globalNamespace: globalNamespace, rootDirectory: rootDirectory);

            List<FileCreationArgs> coreFileCreationsArgs = new List<FileCreationArgs>
            {
                new FileCreationArgs()
                {
                    Filename = "IRepository.cs",
                    PathsToFile = new string[] { "Interfaces", "Repositories" },
                    Template = Templates.IRepositoryTemplate
                },

                new FileCreationArgs()
                {
                    Filename = "CommonEntity.cs",
                    PathsToFile = new string[] { "Entitys" },
                    Template = Templates.CommonEntityTemplate
                },
            };

            CreationExecution infraProcess = new CreationExecution(rootDirectory, infrastructureProperties, infraFileCreationsArgs, packages);

            infraProcess.Execute();

            CreationExecution coreProcess = new CreationExecution(rootDirectory, coreProperties, coreFileCreationsArgs, new string[] { });

            coreProcess.Execute();
        }
    }
}

