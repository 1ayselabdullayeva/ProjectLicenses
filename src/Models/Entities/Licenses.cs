using Models.Common;
using Models.Enums;

namespace Models.Entities
{
	public class Licenses : BaseEntity<int>
	{
		
		public DateTime ExpireDate { get; set; }
		public DateTime ActivationDate { get; set; }
		public LiscenseStatus LicenseStatus { get; set; }
		public int? ProductId { get; set; }
		public Product Product { get; set; }
		public ICollection<Ticket> Ticket { get; set; }
		public ICollection<User> Users { get; set; }

	}
}
