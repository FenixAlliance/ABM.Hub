using System;
using System.Linq;
using FenixAlliance.ABM.Data;
using FenixAlliance.ACL.Configuration.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FenixAlliance.ABM.Hub.Extensions
{
    public static class AllianceBusinessModelExtensions
    {
        public static void AddAllianceBusinessModelServices(this IServiceCollection services, IConfiguration Configuration, IHostEnvironment Environment, ISuiteOptions Options)
        {
                switch (Options.ABM.Provider)
                {
                    case "MSSQL":
                        // Use MSSQL DB
                        services.AddMSSQL(Configuration, Environment, Options, Environment.IsDevelopment());
                        break;

                    case "MySQL":
                        // Use MySQL DB
                        services.AddMySQL(Configuration, Environment, Options, Environment.IsDevelopment());
                        break;

                    default:
                        // Use MySQL DB
                        services.AddMySQL(Configuration, Environment, Options, Environment.IsDevelopment());
                        break;
                }

                if(Environment.IsDevelopment())
                    services.AddDatabaseDeveloperPageExceptionFilter();

        }


        public static void AddMySQL(this IServiceCollection services, IConfiguration Configuration, IHostEnvironment Environment, ISuiteOptions Options, bool Development)
        {
            var Provider = Options.ABM.Providers.Last(c =>
                c.Name == "MSSQL" && c.Purpose == "ABM.Data" && c.Environment == ((!Development) ? "Production" : "Development"));

            // Use Development MySQL DB
            services.AddDbContextPool<ABMContext>(
                options =>
                {
                    options.UseMySql(Provider.ConnectionString, ServerVersion.AutoDetect(Provider.ConnectionString),
                        optionsBuilder =>
                        {
                            optionsBuilder.MigrationsAssembly("FenixAlliance.ABM.Data.MySQL");
                            optionsBuilder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                        });
                });
        }

        public static void AddMSSQL(this IServiceCollection services, IConfiguration Configuration, IHostEnvironment Environment, ISuiteOptions Options, bool Development)
        {
            var Provider = Options.ABM.Providers.Last( c => c.Name == "MSSQL" && c.Purpose == "ABM.Data" && c.Environment == ((!Development) ? "Production" : "Development"));

            // Use Development MSSQL DB
            services.AddDbContext<ABMContext>(options =>
            {
                options.UseSqlServer(Provider.ConnectionString,
                        optionsBuilder =>
                        {
                            optionsBuilder.MigrationsAssembly("FenixAlliance.ABM.Data.MSSQL");
                            optionsBuilder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                        });
            });
        }
    }
}
