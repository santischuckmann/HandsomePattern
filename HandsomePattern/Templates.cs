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
    }
}
