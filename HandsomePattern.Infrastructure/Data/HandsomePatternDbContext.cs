
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;

namespace HandsomePattern.Infrastructure.Data
{
    public partial class HandsomePatternContext : DbContext
    {
        public HandsomePatternContext()
        {

        }

        public HandsomePatternContext(DbContextOptions<HandsomePatternContext> options) : base(options)
        {
        }

        #region DbSets
        
        
        #endregion
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=GestionNutricion;Integrated Security=true;Trust Server Certificate=true;");
        }
    }
}


