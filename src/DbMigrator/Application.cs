using Database;
using DbMigrator.Interfaces;
using DbMigrator.Options;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DbMigrator
{
    /// <summary>
    /// An entry point of DbMigrator execution
    /// </summary>
    internal sealed class Application : IHostedService
    {
        private readonly IEnumerable<ISeeder> _seeders;
        private readonly IHostApplicationLifetime _host;
        private readonly ILogger<Application> _logger;
        private readonly AppDbContext _db;
        private readonly string? _targetMigration;
        private IDbContextTransaction? _transaction;

        public Application(
            ISeedersContainer seedersContainer,
            IHostApplicationLifetime host,
            ILogger<Application> logger,
            AppDbContext db,
            IOptions<TargetMigrationOptions> targetMigrationOptions)
        {
            _seeders = seedersContainer.Seeders;
            _host = host;
            _logger = logger;
            _db = db;
            _targetMigration = targetMigrationOptions.Value.TargetMigration;
        }

        /// <summary>
        /// Applies migrations against an existing database and seeds the data
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _transaction = _db.Database.BeginTransaction();
            try
            {
                await MigrateAsync(cancellationToken);
                await SeedAsync(cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Changes will be rolled back due to the cancellation request");
                    _transaction.Rollback();
                }
                else
                    _transaction.Commit();
            }
            catch (Exception)
            {
                _logger.LogError("Changes will be rolled back due to an exception");
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }

            _host.StopApplication(); // invokes StopAsync() to finish the execution gracefully
        }

        /// <summary>
        /// Stops migration and seeding, reverting the transaction if the operation was not completed
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_transaction is not null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
            }

            return Task.CompletedTask;
        }

        private async Task MigrateAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Migrations started...");

            await _db.Database.GetService<IMigrator>().MigrateAsync(_targetMigration, cancellationToken);

            _logger.LogInformation($"Migrations succeeded!");
        }

        private async Task SeedAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Seeders started...");

            foreach (var seeder in _seeders)
            {
                var alreadySeeded = await seeder.AlreadySeededAsync(_db, cancellationToken);
                if (alreadySeeded)
                    continue;

                _logger.LogInformation("Seeder: {SeederName}...", seeder.GetType().Name);
                await seeder.SeedAsync(_db, cancellationToken);
            }

            _logger.LogInformation($"Seeders succeeded!");
        }
    }
}
