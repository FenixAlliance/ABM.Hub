using FenixAlliance.ABM.Data;
using FenixAlliance.ABM.Data.Access.Clients;
using FenixAlliance.ABM.Data.Access.Context;
using FenixAlliance.ABM.Data.Access.Helpers;
using FenixAlliance.ABM.Data.Interfaces.Access;
using FenixAlliance.ABM.Data.Interfaces.Helpers;
using FenixAlliance.ABM.Data.Interfaces.Storage;
using FenixAlliance.ABM.Data.Seeding.Helpers;
using FenixAlliance.ABM.Data.Seeding.Models;
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

            services.AddTransient<IAccountUsersHelpers, HolderHelpers>();
            services.AddTransient<IHolderDataAccess, HolderDataAccessClient>();

            services.AddTransient<IBusinessDataAccess, TenantDataAccessClient>();
            services.AddTransient<ITenantHelpers, TenantHelpers>();

            services.AddTransient<ISocialDataAccess, SocialDataAccessClient>();
            services.AddTransient<ISocialHelpers, SocialHelpers>();

            //services.AddTransient<IStockDataAccess, StockDataAccessClient>();
            //services.AddTransient<IStoreHelpers, StoreHelpers>();


            services.AddTransient<IStorageDataAccess, StorageDataAccessClient>();
            services.AddTransient<IBlobStorageDataAccessClient, BlobStorageDataAccessClient>();
            services.AddTransient<IMongoHelpers, MongoHelpers>(); 
            services.AddTransient<ISeedingHelpers, SeedingHelpers>(); 

        }


        public static void AddMySQL(this IServiceCollection services, IConfiguration Configuration, IHostEnvironment Environment, ISuiteOptions Options, bool Development)
        {
            FenixAlliance.ACL.Configuration.Interfaces.ABM.IAllianceBusinessModelProvider Provider;

            if (Options.ABM.Providers.Count() == 1)
            {
                Provider = Options.ABM.Providers.First();
            }
            else
            {
                Provider = Options.ABM.Providers.Last(c => c.ProviderEngine == ACL.Configuration.Enums.AllianceBusinessModelProviderEngine.MySQL && c.ProviderPurpose == ACL.Configuration.Enums.AllianceBusinessModelProviderPurpose.ABM_Data && c.Environment == ((!Development) ? "Production" : "Development")) 
                    ?? Options.ABM.Providers.Last(c => c.ProviderEngine == ACL.Configuration.Enums.AllianceBusinessModelProviderEngine.MySQL && c.ProviderPurpose == ACL.Configuration.Enums.AllianceBusinessModelProviderPurpose.ABM_Data);
            }

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
            FenixAlliance.ACL.Configuration.Interfaces.ABM.IAllianceBusinessModelProvider Provider;

            if (Options.ABM.Providers.Count() == 1)
            {
                Provider = Options.ABM.Providers.First();
            }
            else
            {
                Provider = Options.ABM.Providers.Last(c => c.ProviderEngine == ACL.Configuration.Enums.AllianceBusinessModelProviderEngine.MSSQL && c.ProviderPurpose == ACL.Configuration.Enums.AllianceBusinessModelProviderPurpose.ABM_Data && c.Environment == ((!Development) ? "Production" : "Development"))
                    ?? Options.ABM.Providers.Last(c => c.ProviderEngine == ACL.Configuration.Enums.AllianceBusinessModelProviderEngine.MSSQL && c.ProviderPurpose == ACL.Configuration.Enums.AllianceBusinessModelProviderPurpose.ABM_Data);
            }
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
