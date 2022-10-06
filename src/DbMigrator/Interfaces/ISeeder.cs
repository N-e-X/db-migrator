using Database;

namespace DbMigrator.Interfaces
{
    /// <summary>
    /// An interface for seeding data in a database
    /// </summary>
    internal interface ISeeder
    {
        /// <summary>
        /// Checks whether the data, provided by the seeder, has been already seeded
        /// </summary>
        /// <param name="db">The app database context</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<para/>
        /// The task result contains true if the data has been already seeded; otherwise, false.
        /// </returns>
        Task<bool> AlreadySeededAsync(AppDbContext db, CancellationToken cancellationToken);
        /// <summary>
        /// Seed the data provided by the seeder
        /// </summary>
        /// <param name="db">The app database context</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task SeedAsync(AppDbContext db, CancellationToken cancellationToken);
    }
}
