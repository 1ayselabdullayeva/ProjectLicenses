using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Tickets.Delete
{
    public class TicketDeleteResponseDto
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Subject { get; set; }
        public string Description { get; set; }
        public TicketType TicketType { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public int UserId { get; set; }
        public int LicensesId { get; set; }
    }
}
