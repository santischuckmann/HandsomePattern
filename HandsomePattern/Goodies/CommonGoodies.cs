using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HandsomePattern.Goodies
{
    public class CommonGoodies
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

        public static string GetStringReplacedWithNamespace(string input, string _namespace)
        {
            return input.Replace(String.Format("[[{0}]]", "currentNamespace"), _namespace);
        }
    }
}
