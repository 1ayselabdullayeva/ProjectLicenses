using Application.Services;
using Core.Exceptions;
using Core.Repositories.Specific;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Tickets.Create;
using Models.DTOs.Tickets.Edit;
using Models.DTOs.Tickets.GetAll;
using Models.DTOs.Tickets.GetById;
using Models.Entities;
using System.Net.Sockets;
using System.Security.Claims;

namespace Business.Services
{
	public class TicketService : ITicketServices
	{
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserServices _userService;
        public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository, IUserServices userService)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _userService = userService;

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
                TicketStatus = existingTicket.TicketStatus
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

    }
}


