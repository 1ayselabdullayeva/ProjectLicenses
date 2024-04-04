using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Tickets.GetAll
{
    public class TicketGetAllResponseDto
    {
        public int Id { get; set; }
        [Timestamp]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string Subject { get; set; }
        public string Description { get; set; }
        public string TicketType { get; set; }
        public string TicketStatus { get; set; }
        public int? UserId { get; set; }
        public string UserEmail { get; set; }
    }
}
