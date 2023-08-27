using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsomePattern
{
    public static class Templates
    {
        public const string DbContextTemplate = @"
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

        public const string ServiceExtensionsTemplate = @"
using [[currentNamespace]].Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace [[currentNamespace]].Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<[[databaseContext]]>(options =>
               options.UseSqlServer(configuration.GetConnectionString(""[[currentNamespace]]"")), ServiceLifetime.Transient
           );

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            #region Repositories


            #endregion

            #region Services


            #endregion

            #region Handlers


            #endregion

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc(""v1"", new OpenApiInfo { Title = ""[[currentNamespace]] API"", Version = ""v1.0"" });

                doc.AddSecurityDefinition(""Bearer"", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = ""Por favor ingrese un token válido"",
                    Name = ""Authorization"",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = ""JWT"",
                    Scheme = ""bearer""
                });
                doc.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id=""Bearer""
                            }
                        },
                        new string[]{}
                    }
                });
            });

            return services;
        }
    }
}";

        public const string BaseRepositoryTemplate = @"
using [[currentNamespace]].Infrastructure.Data;
using [[currentNamespace]].Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace [[currentNamespace]].Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        #region Variables
        protected readonly [[databaseContext]] _context;
        protected readonly DbSet<T> _entities;
        #endregion

        #region Constructor
        public BaseRepository([[databaseContext]] context)
        {
            this._context = context ?? throw new System.ArgumentNullException(nameof(context));
            _entities = context.Set<T>();
        }
        #endregion

        public IQueryable<T> GetAllQuery()
        {
            return _entities;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task Add(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task AddList(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }

        public void UpdateList(IEnumerable<T> entity)
        {
            _entities.UpdateRange(entity);
        }

        public async Task Delete(int id)
        {
            T entity = await GetById(id);
            _entities.Remove(entity);
        }
    }
}";

        public const string IRepositoryTemplate = @"
using [[currentNamespace]].Core.Entitys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace [[currentNamespace]].Core.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(int id);

        Task Add(T entity);

        void Update(T entity);

        Task Delete(int id);

        Task AddList(IEnumerable<T> entities);

        void UpdateList(IEnumerable<T> entity);
        System.Linq.IQueryable<T> GetAllQuery();
    }
}";

        public const string CommonEntityTemplate = @"
namespace [[currentNamespace]].Core.Entitys
{
    public abstract class CommonEntity
    {
        public int Id { get; set; }

    }
}
";

        public const string IUnitOfWorkTemplate = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace [[currentNamespace]].Core.Interfaces.Repositories
{
    public interface IUnitOfWork: IDisposable
    {
        #region Repositories

        #endregion

        void Save();
        Task SaveAsync();
    }
}
";

        public const string UnitOfWorkTemplate = @"
using [[currentNamespace]].Core.Interfaces.Repositories;
using [[currentNamespace]].Infrastructure.Data;

namespace [[currentNamespace]].Infrastructure.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly [[databaseContext]] _context;
        #region RepositoriesAttributes


        #endregion

        public UnitOfWork([[databaseContext]] context)
        {
            _context = context;
        }

        #region RepositoriesProperties


        #endregion

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
";


    }
}
