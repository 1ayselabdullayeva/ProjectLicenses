using Models.Enums;

namespace Models.DTOs.Tickets.Edit
{
    public class TicketEditStatusResponseDto
    {
        public int Id { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public int UserId;

    }
}
