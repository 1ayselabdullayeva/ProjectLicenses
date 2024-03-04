using Models.Common;
using Models.Enums;

namespace Models.Entities
{
	public class User : BaseEntity<int> { 
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string CompanyName { get; set; }
		public UserStatus Status { get; set; } = UserStatus.Active;
        public string? RefreshToken { get; set; }
        public int RolesId { get; set; } = 2;
		public Roles Roles { get; set; }
		public int? LicensesId { get; set; }
		public Licenses Licenses { get; set; }
		public ICollection<Ticket> Tickets { get; set; }
	}
}
