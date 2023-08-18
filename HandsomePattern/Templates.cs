using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsomePattern
{
    public static class Templates
    {
        public const string UnitOfWorkTemplate = @"
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;

namespace [[currentNamespace]].Infrastructure.Data
{
    public partial class [[currentNamespace]]Context : DbContext
    {
        public [[currentNamespace]]Context()
        {

        }

        public [[currentNamespace]]Context(DbContextOptions<[[currentNamespace]]Context> options) : base(options)
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
            optionsBuilder.UseSqlServer(""[[connectionString]]"");
        }
    }
}

";
    }
}
