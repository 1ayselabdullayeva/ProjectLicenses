namespace Models.Common
{
	public class BaseEntity<Tkey> where Tkey : unmanaged
	{
		public Tkey Id { get; set; }
	}

}
