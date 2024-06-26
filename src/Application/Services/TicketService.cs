﻿using Core.Repositories.Specific;
using Core.Services;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.DTOs.Tickets.Create;
using Models.DTOs.Tickets.Delete;
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
        private readonly IUserRepository _userRepository;
        public TicketService(ITicketRepository ticketRepository, ILicensesRepository licensesRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _licensesRepository = licensesRepository;
            _userRepository = userRepository;
        }
        public async Task<TicketCreateResponseDto> Create(int id,TicketCreateDto request)
		{
            var LicensesId = _licensesRepository.GetSingle(x=>x.Id== request.LicensesId);
            
            var newTicket = new Ticket
            {
                Subject = request.Subject,
                Description = request.Description,
                CreatedAt =request.CreatedAt ,
                TicketStatus = TicketStatus.ToDo,
                TicketType = request.TicketType,
                UserId=id,
                LicensesId= LicensesId.Id
            };
             await _ticketRepository.AddAsync(newTicket);
            var responseDto = new TicketCreateResponseDto
            {
                Subject = newTicket.Subject,
                Description = newTicket.Description,
                CreatedAt= request.CreatedAt,
            };
            return responseDto;
        }

        public TicketDeleteResponseDto DeleteTicket(int id)
        {
            var ticket = _ticketRepository.GetSingle( x=>x.Id== id);                
             _ticketRepository.Remove(ticket);
            _ticketRepository.Save();
            var response = new TicketDeleteResponseDto
            {
                CreatedAt = ticket.CreatedAt,
                Subject = ticket.Subject,
                Description = ticket.Description,
                UserId = ticket.UserId,
            };
            return response;
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
                UserId = existingTicket.UserId,
            };
        }
        public List<TicketGetAllResponseDto> GetAll()
        {
          
            var ticketsWithUser = _ticketRepository.GetAll()
                .Join(_userRepository.GetAll(),
          ticket => ticket.UserId,
          user => user.Id,
          (ticket, user) => new TicketGetAllResponseDto
          {
              Id = ticket.Id,
              CreatedAt = ticket.CreatedAt,
              Subject = ticket.Subject,
              Description = ticket.Description,
              TicketStatus = ticket.TicketStatus.ToString(),
              UserEmail = user.Email
          })
         .ToList();
            return ticketsWithUser;
        }
        public List<TicketGetByIDResponseDto> GetById(int id)
        {
            var ticket = _ticketRepository.GetAll(m => m.UserId == id).ToList();
            var responseList = new List<TicketGetByIDResponseDto>();
            foreach(var item in ticket)
            {
                var response = new TicketGetByIDResponseDto {
                    Id = item.Id,
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
             var ticketsWithUser = _ticketRepository.GetTickets(prodParam)
                .Join(_userRepository.GetAll(),
              ticket => ticket.UserId,
              user => user.Id,
              (ticket, user) => new TicketGetAllResponseDto
              {
                  Id = ticket.Id,
                  CreatedAt = ticket.CreatedAt,
                  Subject = ticket.Subject,
                  Description = ticket.Description,
                  TicketStatus = ticket.TicketStatus.ToString(),
                  UserEmail = user.Email
              })
              .ToList();

            return ticketsWithUser;
        }

        public List<string> GetTicketStatus()
        {
            return Enum.GetNames(typeof(TicketStatus)).ToList();
        }

        public List<string> GetTicketTypes()
        {
            return Enum.GetNames(typeof(TicketType)).ToList();
        }
    }
}


