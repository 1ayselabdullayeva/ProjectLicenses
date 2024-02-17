using Models.Entities;
using Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.Tickets.Create
{
	public class TicketCreateResponseDto
	{
        [Timestamp]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string Subject { get; set; }
        public string Description { get; set; }
        public TicketType TicketType { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public int UserId { get; set; }
        //public static TicketCreateResponseDto Created(Ticket model)
        //{
        //    return new TicketCreateResponseDto
        //    {
        //        Subject = model.Subject,
        //        Description = model.Description,
        //        TicketType = model.TicketType,
        //        TicketStatus = model.TicketStatus,
        //        UserId = model.UserId,
        //    };
        //}

      
    }
}
