namespace Database.Models
{
    public sealed class Group
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = default!;
        public List<GroupUser> GroupUsers { get; set; } = new();
    }
}
