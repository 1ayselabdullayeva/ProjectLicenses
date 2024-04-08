using Models.Common;
namespace Models.Entities
{
    public class Permissions : BaseEntity<int>
    {
        public string PermissionName { get; set; }
        public int RolesId { get; set; }
        public Roles Roles { get; set; }
    }
}
