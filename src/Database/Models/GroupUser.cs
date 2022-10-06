﻿namespace Database.Models
{
    public sealed class GroupUser
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid GroupId { get; set; }
        public Group? Group { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
