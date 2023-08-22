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

            string[] packages = new string[] { "Microsoft.EntityFrameworkCore", "Microsoft.EntityFrameworkCore.SqlServer", "Microsoft.OpenApi" };

            ProjectProperties projectProperties = new ProjectProperties(projectName: "Infrastructure", globalNamespace: globalNamespace, rootDirectory: rootDirectory);

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

            CreationExecution process = new CreationExecution(rootDirectory, projectProperties, fileCreationsArgs, packages);

            process.Execute();
        }
    }
}

