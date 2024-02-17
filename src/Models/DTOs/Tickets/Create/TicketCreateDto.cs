using Models.Entities;
using Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Tickets.Create
{
	public class TicketCreateDto
	{
        [Timestamp]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string Subject { get; set; }
        public string Description { get; set; }
        public TicketType TicketType { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public int UserId { get; set; }
        //public Ticket ToEntity()
        //{
        //    return new Ticket
        //    {
        //        Subject = this.Subject,
        //        Description = this.Description,
        //        TicketType = this.TicketType,
        //        TicketStatus = this.TicketStatus,
        //        UserId= this.UserId
        //    };
        //}

    }
}
