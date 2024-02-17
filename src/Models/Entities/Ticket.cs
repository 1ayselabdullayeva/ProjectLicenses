using Models.Common;
using Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
	public class Ticket : BaseEntity<int>
	{
		public int UserId { get; set; }
		public User User { get; set; }
		[Timestamp]
		public DateTime? CreatedAt { get; set; } = DateTime.Now;
		public string Subject { get; set; }
		public string Description { get; set; }
		public TicketType TicketType { get; set; }
		public TicketStatus TicketStatus { get; set; } = TicketStatus.ToDo;
		public int? LicensesId { get; set; }
		public Licenses Licenses { get; set; }

	}
}
