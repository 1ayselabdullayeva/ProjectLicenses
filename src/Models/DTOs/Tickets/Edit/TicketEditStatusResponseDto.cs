using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Tickets.Edit
{
    public class TicketEditStatusResponseDto
    {
        public int Id { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public int UserId;

    }
}
