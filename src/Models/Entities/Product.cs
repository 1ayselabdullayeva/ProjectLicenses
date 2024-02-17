using Models.Common;

namespace Models.Entities
{
	public class Product : BaseEntity<int>
	{
		public string ProductName { get; set; }
		public ICollection<Licenses>? Licenses { get; set; }
       

    }
}
