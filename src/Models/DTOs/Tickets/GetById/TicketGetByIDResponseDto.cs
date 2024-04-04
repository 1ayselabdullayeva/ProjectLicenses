using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Tickets.GetById
{
    public class TicketGetByIDResponseDto
    {
        [Timestamp]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
	public string Subject { get; set; }
    public string Description { get; set; }
    public string TicketType { get; set; }
    public string TicketStatus { get; set; }
    public int? UserId { get; set; }

    }
}
