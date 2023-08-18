// See https://aka.ms/new-console-template for more information
using System.Text;
using System.Text.RegularExpressions;

namespace HandsomePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            PatternConfiguration configuration = new PatternConfiguration();

            configuration.GlobalNamespace =  typeof(Program).Namespace;

            string rootDirectory = Directory.GetCurrentDirectory();
            if (rootDirectory.Contains("Release") || rootDirectory.Contains("Debug"))
            {
                rootDirectory = rootDirectory.Split("bin")[0];
            }

            configuration.RootDirectory = rootDirectory;

            // infrastructure flow

            string infrastructurePath = Path.Combine(configuration.RootDirectory, "Infrastructure");
            bool hasInfrastuctureDirectory = Directory.GetDirectories(configuration.RootDirectory).ToList().Contains(Path.Combine(configuration.RootDirectory, "Infrastructure"));

            string[] packages = { "Microsoft.EntityFrameworkCore", "Microsoft.EntityFrameworkCore.SqlServer" };
            if (hasInfrastuctureDirectory)
            {
                string dataPath = Path.Combine(infrastructurePath, "Data");
                bool hasDataDirectory = Directory.GetDirectories(infrastructurePath).ToList().Contains(dataPath);

                if (hasDataDirectory)
                {

                    FileCreation fileCreation = new FileCreation(Templates.UnitOfWorkTemplate, Path.Combine(dataPath, "[[currentNamespace]]DbContext.cs"), packages, configuration);
                    fileCreation.CheckDependencies();
                    fileCreation.Create();
                }
            }
        }
    }
}

