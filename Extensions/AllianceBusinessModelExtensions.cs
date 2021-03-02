using FenixAlliance.ABM.Data;
using FenixAlliance.ABM.Data.Access.Clients;
using FenixAlliance.ABM.Data.Access.Context;
using FenixAlliance.ABM.Data.Access.Helpers;
using FenixAlliance.ACL.Configuration.Interfaces;
using FenixAlliance.ACL.Configuration.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace FenixAlliance.ABM.Hub.Extensions
{
    public static class AllianceBusinessModelExtensions
    {
        public static void AddAllianceBusinessModelServices(this IServiceCollection services, IConfiguration Configuration, IHostEnvironment Environment, ISuiteOptions Options = null)
        {

            if (Options == null)
                Options = SuiteOptions.DeserializeOptionsFromFileStatic();


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
            try
            {
                services.AddScoped<ABMContext>(p => p.GetRequiredService<IDbContextLocalFactory<ABMContext>>().CreateDbContext());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            if (Environment.IsDevelopment())
            {
                services.AddDatabaseDeveloperPageExceptionFilter();
            }


            services.AddSingleton<BlobStorageDataAccessClient>();
            services.AddSingleton<HolderDataAccessClient>();
            services.AddSingleton<SocialDataAccessClient>();
            services.AddSingleton<StockDataAccessClient>();
            services.AddSingleton<TenantDataAccessClient>();
            services.AddSingleton<StorageDataAccessClient>();

            services.AddSingleton<HolderHelpers>();
            services.AddSingleton<MongoHelpers>();
            services.AddSingleton<StoreHelpers>();
            services.AddSingleton<SocialHelpers>();
            services.AddSingleton<TenantHelpers>();

        }


        public static void AddMySQL(this IServiceCollection services, IConfiguration Configuration, IHostEnvironment Environment, ISuiteOptions Options, bool Development)
        {
            var Provider = Options.ABM.Providers.Last(c =>
                c.Name == "MySQL" && c.Purpose == "ABM.Data" && c.Environment == ((!Development) ? "Production" : "Development"));

            // Use MySQL DB
            services.AddDbContextLocalFactory<ABMContext>(
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
            var Provider = Options.ABM.Providers.Last(c => c.Name == "MSSQL" && c.Purpose == "ABM.Data" && c.Environment == ((!Development) ? "Production" : "Development"));

            // Use MSSQL DB
            services.AddDbContextLocalFactory<ABMContext>(options =>
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
