namespace Report_A_Crime.Models.Entities
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = default!;
        public ICollection<User> Users { get; set;} = new HashSet<User>();
    }
}
