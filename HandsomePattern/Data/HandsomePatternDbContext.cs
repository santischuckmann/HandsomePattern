
using HandsomePattern.Entitys;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;

namespace HandsomePattern.Infrastructure.Data
{
    public partial class HandsomePatternContext : DbContext
    {
        public DbSet<FileDB> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=HandsomePattern;Integrated Security=true;Trust Server Certificate=true;");
        }
    }
}


