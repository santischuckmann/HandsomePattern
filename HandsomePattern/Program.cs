// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

            // infrastructure flow

            string projectName = "Infrastructure";
            string projectFolder = $"{globalNamespace}.Infrastructure";
            string projectPath = Path.Combine(rootDirectory, projectFolder);
            bool hasFoundProject = Directory.GetDirectories(rootDirectory).ToList().Contains(projectPath);

            if (!hasFoundProject) throw new Exception("Error al buscar el projecto: " + projectFolder);

            string projectNamespace = $"{globalNamespace}.{projectName}";

            DirectoryFinder.CheckDependencies(projectPath, projectNamespace, new string[] { "Microsoft.EntityFrameworkCore", "Microsoft.EntityFrameworkCore.SqlServer" });

            List<FileCreationArgs> fileCreationsArgs = new List<FileCreationArgs>
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
                }
            };

            foreach (FileCreationArgs _args in fileCreationsArgs) 
            {
                var matcher = DirectoryFinder.HasDirectory(projectPath, _args.PathsToFile);

                if (matcher.hasMatched)
                {
                    FileCreation fileCreation = new FileCreation(_args.Template, Path.Combine(matcher.lastPath, _args.Filename));
                    fileCreation.Create();
                }
            }
        }
    }
}

