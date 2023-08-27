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

    public class DirectoryFinder
    {
        public static bool HasDirectory(string rootDirectory, string[] paths)
        {
            if (paths.Length < 1) return false;

            int i = 0;
            string path = string.Empty;
            bool matchingPath = true;
            while (i < paths.Length && matchingPath)
            {
                string acummulatedPath = i >= 1 ? $"{paths[i - 1]}\\{paths[i]}" : paths[i];
                string accumulatedRootDirectory = i >= 1 ? $"{rootDirectory}\\{paths[i - 1]}" : rootDirectory;
                path = Path.Combine(rootDirectory, acummulatedPath);
                matchingPath = Directory.GetDirectories(accumulatedRootDirectory).ToList().Contains(path);
                i++;
            }

            return matchingPath;
        }

        public static void CheckDependencies(string rootDirectory, string projectNamespace, string[] packages)
        {
            using (StreamReader reader = new StreamReader(Path.Combine(rootDirectory, $"{projectNamespace}.csproj")))
            {

                string line;
                string packageNamePattern = @"(?<=Include="")[\w|.]+(?="")";

                List<string> packagesInProject = new List<string>();

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("PackageReference"))
                        packagesInProject.Add(Regex.Match(line, packageNamePattern).Value);
                }

                foreach (string package in packages)
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
    }

    public class CSharpClassContent
    {
        public CSharpClassContent()
        {

        }
    }

    public class FileCreationArgs
    {
        public string Template { get; set; }
        public string Filename { get; set; }
        public string[] PathsToFile { get; set; }
    }

    public class CSProjectConfiguration
    {
        public string Namespace { get; set; }
        public string RootDirectory { get; set; }
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
        }

        public string GlobalNamespace { get; set; }
        public string ProjectName { get; set; }
        public string ProjectPath { get; set; }
        public string ProjectFolder { get; set; }
        public string ProjectNamespace { get; set; }
    }

    public class CreationExecution
    {
        private string _rootDirectory;
        private ProjectProperties _projectProperties;
        private List<FileCreationArgs> _fileCreationArgs;
        private string[] _packages;
        private string[] _references;

        public CreationExecution(string rootDirectory, ProjectProperties projectProperties, List<FileCreationArgs> fileCreationArgs, string[] packages, string[] references)
        {
            _projectProperties = projectProperties;
            _fileCreationArgs = fileCreationArgs;
            _rootDirectory = rootDirectory;
            _packages = packages;
            _references = references;
        }

        public void Execute()
        {
            bool hasFoundProject = Directory.GetDirectories(_rootDirectory).ToList().Contains(_projectProperties.ProjectPath);

            if (!hasFoundProject) throw new Exception("Error al buscar el projecto: " + _projectProperties.ProjectFolder);



            XmlDocument doc = new XmlDocument();
            string csprojPath = Path.Combine(_projectProperties.ProjectPath, $"{_projectProperties.ProjectNamespace}.csproj");
            doc.Load(csprojPath);

            XmlNodeList itemGroupNodes = doc.SelectNodes("//ItemGroup");

            XmlNode firstItemGroupNode = itemGroupNodes[0];

            foreach (string reference in _references)
            {
                if (firstItemGroupNode != null)
                {
                    XmlNode itemGroupReference = doc.CreateElement("ItemGroup");
                    XmlNode referenceNode = doc.CreateElement("ProjectReference");

                    // Set attributes for the Reference node
                    XmlAttribute includeAttribute = doc.CreateAttribute("Include");
                    includeAttribute.Value = reference; // Update with the actual project name
                    referenceNode.Attributes.Append(includeAttribute);

                    itemGroupReference.AppendChild(referenceNode);

                    firstItemGroupNode.ParentNode.InsertAfter(itemGroupReference, firstItemGroupNode);
                    doc.Save(csprojPath);
                }
            }

            DirectoryFinder.CheckDependencies(_projectProperties.ProjectPath, _projectProperties.ProjectNamespace, _packages);

            foreach (FileCreationArgs _args in _fileCreationArgs)
            {
                var hasMatched = DirectoryFinder.HasDirectory(_projectProperties.ProjectPath, _args.PathsToFile);

                var pathForFile = CreatePathForFile(_args.PathsToFile);

                if (!hasMatched)
                    CreateDirectoriesForFile(_args.PathsToFile);

                FileCreation fileCreation = new FileCreation(_args.Template, Path.Combine(pathForFile, _args.Filename), _projectProperties.GlobalNamespace);
                fileCreation.Create();

            }
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
