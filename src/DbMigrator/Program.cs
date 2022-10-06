using Database;
using DbMigrator;
using DbMigrator.Interfaces;
using DbMigrator.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostContext, configBuilder) =>
        {
            configBuilder.AddUserSecrets<Program>(optional: true);
        })
        .ConfigureServices((hostContext, services) =>
        {
            services.AddHostedService<Application>();

            var configuration = hostContext.Configuration;
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbConnection")),
                contextLifetime: ServiceLifetime.Singleton,
                optionsLifetime: ServiceLifetime.Singleton);

            services.AddSingleton<ISeedersContainer, SeedersContainer>();
            services.AddOptions<TargetMigrationOptions>().Bind(configuration);
        });
