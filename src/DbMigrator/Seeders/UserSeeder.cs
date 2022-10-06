using Database;
using Database.Models;
using DbMigrator.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DbMigrator.Seeders
{
    internal sealed class UserSeeder : ISeeder
    {
        public async Task<bool> AlreadySeededAsync(AppDbContext db, CancellationToken cancellationToken)
        {
            return await db.Users.AnyAsync(cancellationToken);
        }

        public async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Id = Guid.Parse("67854e4c-3b0b-4531-b2ab-60053cc658bf"),
                Email = "john_snow@email.de",
                Name = "John Snow",
            };

            await db.AddAsync(user, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
