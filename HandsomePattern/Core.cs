using HandsomePattern.Enums;
using HandsomePattern.Goodies;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace HandsomePattern
{
    public class FileCreation
    {
        private string _fileContent;
        private string _path;
        private string _namespace;

        public FileCreation(string fileContent, string path, string Namespace)
        {
            _fileContent = fileContent;
            _path = path;
            _namespace = Namespace;
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

        public void AppendVariables()
        {
            string newFileContent = _fileContent;
            string newPath = _path;

            List<Tuple<string, string>> tupleList = new List<Tuple<string, string>>()
            {
                Tuple.Create("currentNamespace", _namespace),
                Tuple.Create("connectionString", "Server=localhost;Database=GestionNutricion;Integrated Security=true;Trust Server Certificate=true;"),
                Tuple.Create("databaseContext", $"{_namespace}Context")
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

    public class FileCreationArgs
    {
        public string Template { get; set; }
        public string Filename { get; set; }
        public string[] PathsToFile { get; set; }
        public DependencyType DependencyType { get; set; } = DependencyType.None;
    }

    public class ProjectProperties
    {
        public ProjectProperties(string projectName, string globalNamespace, string rootDirectory)
        {
            GlobalNamespace = globalNamespace;
            ProjectName = projectName;
            var projectFolder = $"{globalNamespace}.{projectName}";
            ProjectFolder = projectFolder;
            ProjectPath = Path.Combine(rootDirectory, projectFolder);
            ProjectNamespace = projectFolder;
            ExtensionPath = Path.Combine(ProjectPath, "Extensions", "ServiceCollectionExtensions.cs");
        }

        public string GlobalNamespace { get; set; }
        public string ProjectName { get; set; }
        public string ProjectPath { get; set; }
        public string ProjectFolder { get; set; }
        public string ProjectNamespace { get; set; }
        public string ExtensionPath { get; set; }
    }

    public class CreationExecution
    {
        private ProjectProperties _projectProperties;
        private List<FileCreationArgs> _fileCreationArgs;

        public CreationExecution(ProjectProperties projectProperties, List<FileCreationArgs> fileCreationArgs)
        {
            _projectProperties = projectProperties;
            _fileCreationArgs = fileCreationArgs;
        }

        public void Execute()
        {
            foreach (FileCreationArgs _args in _fileCreationArgs)
            {
                var pathForFile = CreatePathForFile(_args.PathsToFile);

                bool isAlreadyCreated = IsFileAlreadyCreated(_args.Filename, pathForFile);

                if (isAlreadyCreated) continue;

                var hasMatched = CommonGoodies.HasDirectory(_projectProperties.ProjectPath, _args.PathsToFile);

                if (!hasMatched)
                    CreateDirectoriesForFile(_args.PathsToFile);

                FileCreation fileCreation = new FileCreation(_args.Template, Path.Combine(pathForFile, _args.Filename), _projectProperties.GlobalNamespace);
                fileCreation.Create();

                string section = GetSectionByDependencyType(_args.DependencyType);

                if (section.Length > 0)
                {
                    string extensionsContent = File.ReadAllText(_projectProperties.ExtensionPath);

                    string regionPattern = @"#region\s+" + Regex.Escape(section) + @"(?<content>.*?)#endregion";
                    Match regionMatch = Regex.Match(extensionsContent, regionPattern, RegexOptions.Singleline);

                    if (regionMatch.Success)
                    {
                        string newContent = "services.AddScoped<IUnitOfWork, UnitOfWork>();";

                        string oldRegionContent = regionMatch.Groups["content"].Value;

                        string identation = oldRegionContent.Replace("\r", "").Replace("\n", "");

                        StringBuilder newRegionContent = new StringBuilder()
                            .Append(oldRegionContent)
                            .Append(newContent)
                            .Append(Environment.NewLine)
                            .Append(Environment.NewLine)
                            .Append(identation);

                        //string newRegionContent = oldRegionContent + Environment.NewLine + identation + newContent;

                        string modifiedFileContent = extensionsContent.Replace(oldRegionContent, newRegionContent.ToString());
                        File.WriteAllText(_projectProperties.ExtensionPath, modifiedFileContent);
                    }
                }
            }
        }

        private string GetSectionByDependencyType(DependencyType dependencyType)
        {
            switch (dependencyType) 
            {
                case DependencyType.Repository:
                    return "Repositories";
                case DependencyType.Service:
                    return "Services";
                case DependencyType.Handler:
                    return "Handlers";
                case DependencyType.None:
                default:
                    return "";
            }
        }

        private bool IsFileAlreadyCreated(string rawFilename, string pathForFile)
        {
            string filename = CommonGoodies.GetStringReplacedWithNamespace(rawFilename, _projectProperties.GlobalNamespace);

            return File.Exists(Path.Combine(pathForFile, filename));
        }

        private string CreatePathForFile(string[] pathsToFile)
        {
            string finalPath = _projectProperties.ProjectPath;
            foreach (string path in pathsToFile)
            {
                finalPath = Path.Combine(finalPath, path);
            }

            return finalPath;
        }

        private void CreateDirectoriesForFile(string[] pathsToFile)
        {
            string rootDirectory = _projectProperties.ProjectPath;

            bool insideNonCreatedDirectory = false;
            foreach (string path in pathsToFile)
            {
                string newPath = Path.Combine(rootDirectory, path);
                bool hasBeenCreated = Directory.GetDirectories(rootDirectory).ToList().Contains(newPath);

                if (!hasBeenCreated || insideNonCreatedDirectory)
                {
                    Directory.CreateDirectory(newPath);
                    insideNonCreatedDirectory = true;
                }

                rootDirectory = newPath;
            }
        }
    }
}
