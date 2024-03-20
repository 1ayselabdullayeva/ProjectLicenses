using Models.Entities;
using Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Tickets.Create
{
	public class TicketCreateResponseDto
	{
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string Subject { get; set; }
        public string Description { get; set; }
        public TicketType TicketType { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public int UserId { get; set; }
        public int LicensesId { get; set; }
    }
}
