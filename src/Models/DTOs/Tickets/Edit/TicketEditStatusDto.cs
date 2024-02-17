using Microsoft.Identity.Client;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Tickets.Edit
{
    public class TicketEditStatusDto
    {
        public int Id { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public int UserId;
    }
}
