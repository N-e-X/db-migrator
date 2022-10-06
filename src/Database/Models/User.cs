namespace Database.Models
{
    public sealed class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public List<GroupUser> UserGroups { get; set; } = new();
    }
}
