using Application.FluentValidation;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Tickets.Create;
using Models.DTOs.Tickets.Edit;
using Models.Entities;
using System.Security.Claims;

namespace Api.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketServices _ticketService;
        private readonly IUserServices _userServices;

        public TicketController(ITicketServices ticketService, IUserServices userServices)
        {
            _ticketService = ticketService;
            _userServices = userServices;
        }

        [Authorize("Customer")]
        [HttpGet("getbyid")]
        public IActionResult GetTicketById()
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var responseDto = _ticketService.GetById(id);
            return Ok(responseDto);
        }


        [Authorize("Admin")]
        [HttpPut("edit")]
        public IActionResult EditTicketStatus([FromBody] TicketEditStatusDto request)
        {
            TicketValidator tv= new TicketValidator();
            var validationResult = tv.Validate(new Ticket
            {
                 TicketStatus= request.TicketStatus
            });
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var responseDto = _ticketService.Edit(request);
            return Ok(responseDto);
        }
        [Authorize("Admin")]
        [HttpGet("getAllTickets")]
        public IActionResult GetAllTickets()
        {
            var response = _ticketService.GetAll();
            return Ok(response);
        }
        [Authorize("Customer")]
        [HttpPost("CreateTicket")]
        public async Task<IActionResult> Create(TicketCreateDto request) 
        {
             var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            TicketValidator tv = new TicketValidator();
            var validationResult = tv.Validate(new Ticket
            {
                Description = request.Description,
                TicketStatus = request.TicketStatus,
                TicketType = request.TicketType,
                CreatedAt = request.CreatedAt,
                UserId= userId
            });
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            
            var response = await _ticketService.Create(userId, request);
            return Ok(response);
        }
    }
}

