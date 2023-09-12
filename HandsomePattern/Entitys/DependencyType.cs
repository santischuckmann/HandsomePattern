using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsomePattern.Entitys
{
    public class DependencyType
    {
        public DependencyType() 
        {
            Files = new HashSet<FileDB>();
        }
        public int Id { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public virtual ICollection<FileDB> Files { get; set; }
    }
}
