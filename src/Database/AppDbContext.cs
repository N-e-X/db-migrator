using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public sealed class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Group> Groups { get; set; } = default!;
        public DbSet<GroupUser> GroupsUsers { get; set; } = default!;
        public DbSet<Role> Roles { get; set; } = default!;

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(IAssemblyMarker).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}