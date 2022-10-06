using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    internal sealed class GroupUserConfiguration : IEntityTypeConfiguration<GroupUser>
    {
        public void Configure(EntityTypeBuilder<GroupUser> builder)
        {
            builder.HasKey(x => new { x.UserId, x.GroupId });

            builder
                .HasOne(groupUser => groupUser.Group)
                .WithMany(group => group.GroupUsers);

            builder
                .HasOne(userGroup => userGroup.User)
                .WithMany(user => user.UserGroups);

            builder
                .HasOne(userGroup => userGroup.Role);
        }
    }
}
