using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsomePattern.Entitys
{
    public class FileDB
    {
        public int FileId { get; set; }
        public string Filename { get; set; }
        public string PathsToFile { get; set; }
        public string Template { get; set; }
        public int DependencyTypeId { get; set; }
        public string ProjectName { get; set; }
        public virtual DependencyType DependencyType { get; set; }
    }
}
