using HandsomePattern.Entitys;
using HandsomePattern.Infrastructure.Data;
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

            string[] packages = new string[] { "Microsoft.EntityFrameworkCore", "Microsoft.EntityFrameworkCore.SqlServer", "Microsoft.OpenApi", "AutoMapper.Extensions.Microsoft.DependencyInjection", "Swashbuckle.AspNetCore.SwaggerGen", "Microsoft.EntityFrameworkCore.Tools" };

            ProjectProperties infrastructureProperties = new ProjectProperties(projectName: "Infrastructure", globalNamespace: globalNamespace, rootDirectory: rootDirectory);

            Project infrastructure = new Project(infrastructureProperties, packages, new string[] { "..\\[[currentNamespace]].Core\\[[currentNamespace]].Core.csproj" });

            infrastructure.DoesProjectExist(rootDirectory);
            infrastructure.CorrectProjectReferences();
            infrastructure.CheckDependencies();

            List<FileCreationArgs> infraFileCreationsArgs = new List<FileCreationArgs>();

            var context = new HandsomePatternContext();

            List<FileDB> filesInfra = context.Files.Where(x => x.ProjectName == "Infrastructure").ToList();

            foreach (FileDB file in filesInfra)
            {
                infraFileCreationsArgs.Add(new FileCreationArgs()
                {
                    DependencyType = (Enums.DependencyType)file.DependencyTypeId,
                    Filename = file.Filename,
                    PathsToFile = file.PathsToFile.Split("//"),
                    Template = file.Template
                });
            }

            ProjectProperties coreProperties = new ProjectProperties(projectName: "Core", globalNamespace: globalNamespace, rootDirectory: rootDirectory);

            Project core = new Project(coreProperties, new string[] { }, new string[] { });

            core.DoesProjectExist(rootDirectory);
            core.CorrectProjectReferences();

            List<FileDB> filesCore = context.Files.Where(x => x.ProjectName == "Core").ToList();

            List<FileCreationArgs> coreFileCreationsArgs = new List<FileCreationArgs>();

            foreach (FileDB file in filesCore)
            {
                coreFileCreationsArgs.Add(new FileCreationArgs()
                {
                    DependencyType = (Enums.DependencyType)file.DependencyTypeId,
                    Filename = file.Filename,
                    PathsToFile = file.PathsToFile.Split("//"),
                    Template = file.Template
                });
            }

            CreationExecution infraProcess = new CreationExecution(infrastructureProperties, infraFileCreationsArgs);

            infraProcess.Execute();

            CreationExecution coreProcess = new CreationExecution(coreProperties, coreFileCreationsArgs);

            coreProcess.Execute();
        }
    }
}

