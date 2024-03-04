using Core.Repositories.Specific;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.DTOs.Tickets.Create;
using Models.DTOs.Tickets.Edit;
using Models.DTOs.Tickets.GetAll;
using Models.DTOs.Tickets.GetById;
using Models.Entities;

namespace Business.Services
{
	public class TicketService : ITicketServices
	{
        private readonly ITicketRepository _ticketRepository;
        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;

        }
        public async Task<TicketCreateResponseDto> Create(int id,TicketCreateDto request)
		{
            //var userId = User.FindFirst(ClaimTypes.Name)?.Value;
            var newTicket = new Ticket
            {
                Subject = request.Subject,
                Description = request.Description,
                UserId=id
            };
             await _ticketRepository.AddAsync(newTicket);
            var responseDto = new TicketCreateResponseDto
            {
                Subject = newTicket.Subject,
                Description = newTicket.Description
            };

            return responseDto;
        }

        public TicketEditStatusResponseDto Edit(TicketEditStatusDto request)
        {
            var existingTicket = _ticketRepository.GetSingle(t => t.Id == request.Id);

            //if (existingTicket == null)
            //{

                
            //}
            existingTicket.TicketStatus = request.TicketStatus;
            _ticketRepository.Edit(existingTicket);
            _ticketRepository.Save();
            return new TicketEditStatusResponseDto
            {
                Id = existingTicket.Id,
                TicketStatus = existingTicket.TicketStatus,
            };
        }

        public List<TicketGetAllResponseDto> GetAll()
        {
           var ticket=_ticketRepository.GetAll();
           var tickets= ticket.Select(t => new TicketGetAllResponseDto
           {
                Id = t.Id,
                CreatedAt = t.CreatedAt,
                Subject = t.Subject,
                Description = t.Description,
                UserId = t.UserId,
                TicketStatus = t.TicketStatus
            }).ToList();

            return tickets;

        }


        public TicketGetByIDResponseDto GetById(int id)
		{
			var ticket = _ticketRepository.GetSingle(m=>m.UserId==id);
            var response = new TicketGetByIDResponseDto
            {
               CreatedAt = ticket.CreatedAt,
               Description = ticket.Description,
               Subject = ticket.Subject,
               TicketStatus = ticket.TicketStatus,
               TicketType= ticket.TicketType
            };
            return response;
        }

        public List<TicketGetAllResponseDto> GetTicketsPagingData([FromQuery] PagedParameters prodParam)
        {
            var tickets = _ticketRepository.GetTickets(prodParam);


            // products dönüştürülmüş bir şekilde döndürülüyor
            var responseDtoList = tickets.Select(p => new TicketGetAllResponseDto
            {
                Id = p.Id,
                CreatedAt = p.CreatedAt,
                Subject = p.Subject,
                TicketStatus = p.TicketStatus,
                TicketType= p.TicketType

            }).ToList();

            return responseDtoList;

        }
    }
}


