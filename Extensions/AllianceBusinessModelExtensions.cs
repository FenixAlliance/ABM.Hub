﻿using System;
using System.Linq;
using FenixAlliance.ABM.Data;
using FenixAlliance.ACL.Configuration.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FenixAlliance.ABM.Hub.Extensions
{
    public static class AllianceBusinessModelExtensions
    {
        public static void AddAllianceBusinessModelServices(this IServiceCollection services, IConfiguration Configuration, IHostEnvironment Environment, SuiteOptions Options)
        {
                switch (Options.ABM.Provider)
                {
                    case "MSSQL":
                        // Use Production MSSQL DB
                        services.AddMSSQL(Configuration, Environment, Options, Environment.IsDevelopment());
                        break;

                    case "MySQL":
                        // Use Production MySQL DB
                        services.AddMySQL(Configuration, Environment, Options, Environment.IsDevelopment());
                        break;

                    default:
                        // Use Production MySQL DB
                        services.AddMySQL(Configuration, Environment, Options, Environment.IsDevelopment());
                        break;
                }


        }


        public static void AddMySQL(this IServiceCollection services, IConfiguration Configuration, IHostEnvironment Environment, SuiteOptions Options, bool Development)
        {
            var Provider = Options.ABM.Providers.Last(c =>
                c.Name == "MSSQL" && c.Purpose == "ABM.Data" && c.Environment == ((!Development) ? "Production" : "Development"));

            // Use Development MySQL DB
            services.AddDbContextPool<ABMContext>(
                options => options.UseMySql(Provider.ConnectionString, ServerVersion.AutoDetect(Provider.ConnectionString),
                b => 
                            {
                                b.MigrationsAssembly("FenixAlliance.ABM.Data.MySQL");
                                b.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                            }));
        }

        public static void AddMSSQL(this IServiceCollection services,
            IConfiguration Configuration, IHostEnvironment Environment, SuiteOptions Options, bool Development)
        {
            var Provider = Options.ABM.Providers.Last(c =>
                c.Name == "MSSQL" && c.Purpose == "ABM.Data" && c.Environment == ((!Development) ? "Production" : "Development"));

            // Use Development MSSQL DB
            services.AddDbContext<ABMContext>(
                options => options.UseSqlServer(
                    Provider.ConnectionString,
                    b => b.MigrationsAssembly("FenixAlliance.ABM.Data.MSSQL")));
        }
    }
}
