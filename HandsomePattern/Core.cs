using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HandsomePattern
{
    public class FileCreation
    {
        private string _fileContent;
        private string _path;
        private string[] _packages;
        private CSProjectConfiguration _configuration;

        public FileCreation(string fileContent, string path, string[] packages, CSProjectConfiguration configuration)
        {
            _fileContent = fileContent;
            _path = path;
            _packages = packages;
            _configuration = configuration;
        }

        public void Create()
        {
            try
            {
                AppendVariables();
                File.WriteAllText(_path, _fileContent + Environment.NewLine);

                Console.WriteLine("File created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public void CheckDependencies()
        {
            using (StreamReader reader = new StreamReader(Path.Combine(_configuration.RootDirectory, $"{_configuration.Namespace}.csproj")))
            {

                string line;
                string packageNamePattern = @"(?<=Include="")[\w|.]+(?="")";

                List<string> packagesInProject = new List<string>();

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("PackageReference"))
                        packagesInProject.Add(Regex.Match(line, packageNamePattern).Value);
                }

                foreach (string package in _packages)
                {
                    bool hasBeenFound = false;
                    foreach (string projectPackage in packagesInProject)
                    {
                        hasBeenFound = hasBeenFound || projectPackage.Equals(package);
                    }

                    if (!hasBeenFound)
                    {
                        throw new Exception("The following required package was not found: " + package);
                    }
                }
            }
        }

        public void AppendVariables()
        {
            Console.WriteLine(typeof(Program).Namespace);
            // es probable que esto tenga que ser leido de un archivo de config
            string newFileContent = _fileContent;
            string newPath = _path;

            List<Tuple<string, string>> tupleList = new List<Tuple<string, string>>()
            {
                Tuple.Create("currentNamespace", typeof(Program).Namespace),
                Tuple.Create("connectionString", "Server=localhost;Database=GestionNutricion;Integrated Security=true;Trust Server Certificate=true;")
            };

            foreach (var tuple in tupleList)
            {
                newFileContent = newFileContent.Replace(String.Format("[[{0}]]", tuple.Item1), tuple.Item2);
                newPath = newPath.Replace(String.Format("[[{0}]]", tuple.Item1), tuple.Item2);
            }

            _fileContent = newFileContent;
            _path = newPath;
        }
    }

    public class DirectoryFinder
    {
        public static (bool hasMatched, string lastPath) HasDirectory(string rootDirectory, string[] paths)
        {
            if (paths.Length < 1) return (false, "");

            int i = 0;
            string path = string.Empty;
            bool matchingPath = true;
            while (i < paths.Length && matchingPath)
            {
                string acummulatedPath = i >= 1 ? $"{paths[i - 1]}\\{paths[i]}" : paths[i];
                string accumulatedRootDirectory = i >= 1 ? $"{rootDirectory}{paths[i - 1]}" : rootDirectory;
                path = Path.Combine(rootDirectory, acummulatedPath);
                matchingPath = Directory.GetDirectories(accumulatedRootDirectory).ToList().Contains(path);
                i++;
            }

            return (matchingPath, path);
        }
    }

    public class CSharpClassContent
    {
        public CSharpClassContent()
        {

        }
    }

    public class CSProjectConfiguration
    {
        public string Namespace { get; set; }
        public string RootDirectory { get; set; }
    }
}
