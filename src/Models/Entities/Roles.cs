using Models.Common;

namespace Models.Entities
{
	public class Roles : BaseEntity<int>
	{
		public string RoleName { get; set; }
		public ICollection<User> Users { get; set; }
	}
}
