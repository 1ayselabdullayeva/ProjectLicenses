using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Tickets.GetAll
{
    public class TicketGetAllResponseDto
    {
        public int Id { get; set; }
        [Timestamp]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string Subject { get; set; }
        public string Description { get; set; }
        public TicketType TicketType { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public int? UserId { get; set; }
    }
}
