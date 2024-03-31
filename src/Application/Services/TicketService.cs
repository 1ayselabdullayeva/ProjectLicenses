using Azure;
using Core.Repositories.Specific;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.DTOs.Licenses.GetById;
using Models.DTOs.Tickets.Create;
using Models.DTOs.Tickets.Edit;
using Models.DTOs.Tickets.GetAll;
using Models.DTOs.Tickets.GetById;
using Models.Entities;
using Models.Enums;

namespace Business.Services
{
	public class TicketService : ITicketServices
	{
        private readonly ITicketRepository _ticketRepository;
        private readonly ILicensesRepository _licensesRepository;
        public TicketService(ITicketRepository ticketRepository, ILicensesRepository licensesRepository)
        {
            _ticketRepository = ticketRepository;
            _licensesRepository = licensesRepository;
        }
        public async Task<TicketCreateResponseDto> Create(int id,int LicenseId,TicketCreateDto request)
		{
            var LicensesId = _licensesRepository.GetSingle(x=>x.Id== LicenseId);
            var newTicket = new Ticket
            {
                Subject = request.Subject,
                Description = request.Description,
                CreatedAt = request.CreatedAt,
                TicketStatus = TicketStatus.ToDo,
                TicketType = request.TicketType,
                UserId=id,
                LicensesId= LicensesId.Id
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
                TicketStatus = t.TicketStatus.ToString(),
            }).ToList();
            return tickets;
        }
        public List<TicketGetByIDResponseDto> GetById(int id)
        {
            var ticket = _ticketRepository.GetAll(m => m.UserId == id).ToList();
            var responseList = new List<TicketGetByIDResponseDto>();
            foreach(var item in ticket)
            {
                var response = new TicketGetByIDResponseDto {
                    CreatedAt = item.CreatedAt,
                    Description = item.Description,
                    Subject = item.Subject,
                    TicketStatus = item.TicketStatus.ToString(),
                    TicketType = item.TicketType.ToString(),
                };
                responseList.Add(response); 
            };
            return responseList;
        }

        public List<TicketGetAllResponseDto> GetTicketsPagingData([FromQuery] PagedParameters prodParam)
        {
            var tickets = _ticketRepository.GetTickets(prodParam);
            var responseDtoList = tickets.Select(p => new TicketGetAllResponseDto
            {
                Id = p.Id,
                CreatedAt = p.CreatedAt,
                Subject = p.Subject,
                TicketStatus = p.TicketStatus.ToString(),
                TicketType= p.TicketType.ToString()

            }).ToList();
            return responseDtoList;
        }
        public List<string> GetTicketTypes()
        {
            return Enum.GetNames(typeof(TicketType)).ToList();
        }
    }
}


