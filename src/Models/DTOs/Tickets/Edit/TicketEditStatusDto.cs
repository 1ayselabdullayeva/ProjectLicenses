using Models.Enums;

namespace Models.DTOs.Tickets.Edit
{
    public class TicketEditStatusDto
    {
        public int Id { get; set; }
        public TicketStatus TicketStatus { get; set; }
    }
}
