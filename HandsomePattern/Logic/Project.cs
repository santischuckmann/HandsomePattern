using HandsomePattern.Goodies;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace HandsomePattern.Logic
{
    public class Project
    {
        private ProjectProperties _projectProperties;
        private string[] _packages;
        private string[] _references;
        public Project(ProjectProperties projectProperties, string[] packages, string[] references) 
        { 
            _projectProperties = projectProperties;
            _packages = packages;
            _references = references;
        }

        public void DoesProjectExist(string rootDirectory)
        {
            bool hasFoundProject = Directory.GetDirectories(rootDirectory).ToList().Contains(_projectProperties.ProjectPath);

            if (!hasFoundProject) throw new Exception("Error al buscar el projecto: " + _projectProperties.ProjectFolder);
        }

        public void CorrectProjectReferences()
        {
            XmlDocument doc = new XmlDocument();
            string csprojPath = Path.Combine(_projectProperties.ProjectPath, $"{_projectProperties.ProjectNamespace}.csproj");
            doc.Load(csprojPath);

            HashSet<string> missingReferences = new HashSet<string>();

            XmlNodeList itemGroupNodes = doc.SelectNodes("//ItemGroup");
            XmlNodeList projectReferences = doc.SelectNodes("//ProjectReference");

            if (projectReferences.Count > 0)
            {
                foreach (XmlNode projRef in projectReferences)
                {
                    foreach (string rawReference in _references)
                    {
                        string reference = CommonGoodies.GetStringReplacedWithNamespace(rawReference, _projectProperties.GlobalNamespace);
                        if (projRef.Attributes["Include"].Value != reference)
                        {
                            missingReferences.Add(reference);
                        }
                    }
                }
            } else
            {
                foreach (string rawReference in _references)
                {
                    string reference = CommonGoodies.GetStringReplacedWithNamespace(rawReference, _projectProperties.GlobalNamespace);
                    missingReferences.Add(reference);
                }
            }


            XmlNode firstItemGroupNode = itemGroupNodes[0];

            foreach (string reference in missingReferences)
            {
                if (firstItemGroupNode != null)
                {
                    XmlNode itemGroupReference = doc.CreateElement("ItemGroup");
                    XmlNode referenceNode = doc.CreateElement("ProjectReference");

                    XmlAttribute includeAttribute = doc.CreateAttribute("Include");
                    includeAttribute.Value = reference;
                    referenceNode.Attributes.Append(includeAttribute);

                    itemGroupReference.AppendChild(referenceNode);

                    firstItemGroupNode.ParentNode.InsertAfter(itemGroupReference, firstItemGroupNode);
                    doc.Save(csprojPath);
                }
            }
        }

        public void CheckDependencies()
        {
            using (StreamReader reader = new StreamReader(Path.Combine(_projectProperties.ProjectPath, $"{_projectProperties.ProjectNamespace}.csproj")))
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

    }
}
