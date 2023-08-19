// See https://aka.ms/new-console-template for more information
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

            CSProjectConfiguration configuration = new()
            {
                RootDirectory = $"{rootDirectory}\\{globalNamespace}.{projectName}",
                Namespace = $"{globalNamespace}.{projectName}"
            };

            var matcher = DirectoryFinder.HasDirectory(projectPath, new string[] { "Data" });

            string[] packages = { "Microsoft.EntityFrameworkCore", "Microsoft.EntityFrameworkCore.SqlServer" };

            if (matcher.hasMatched)
            {
                FileCreation fileCreation = new FileCreation(Templates.UnitOfWorkTemplate, Path.Combine(matcher.lastPath, "[[currentNamespace]]DbContext.cs"), packages, configuration);
                fileCreation.CheckDependencies();
                fileCreation.Create();
            }

        }
    }
}

