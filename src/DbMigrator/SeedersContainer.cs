using DbMigrator.Interfaces;
using DbMigrator.Seeders;

namespace DbMigrator
{
    internal sealed class SeedersContainer : ISeedersContainer
    {
        public IEnumerable<ISeeder> Seeders { get; } = new ISeeder[]
        {
            // order does matter!
            new UserSeeder(),
        };
    }
}
