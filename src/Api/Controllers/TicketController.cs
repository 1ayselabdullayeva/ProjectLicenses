using Application.FluentValidation;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Product.GetAll;
using Models.DTOs;
using Models.DTOs.Tickets.Create;
using Models.DTOs.Tickets.Edit;
using Models.Entities;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Core.Repositories.Specific;
using Models.DTOs.Tickets.GetAll;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : Controller
    {
        private readonly ITicketServices _ticketService;
        private readonly ITicketRepository _ticketRepository;
        public TicketController(ITicketServices ticketService, ITicketRepository ticketRepository)
        {
            _ticketService = ticketService;
            _ticketRepository = ticketRepository;
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

        [HttpGet("PaginationTicket")]
        public ActionResult<List<ProductGetAllResponseDto>> GetProductPagingData([FromQuery] PagedParameters prodParam)
        {
            var tickets = _ticketRepository.GetTickets(prodParam);

            var metadata = new
            {
                tickets.TotalCount,
                tickets.PageSize,
                tickets.CurrentPage,
                tickets.TotalPages,
                tickets.HasNext,
                tickets.HasPrevious
            };

            HttpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var responseDtoList = tickets.Select(p => new TicketGetAllResponseDto
            {
                Id = p.Id,
                CreatedAt=p.CreatedAt,
                Description = p.Description,
                TicketStatus = p.TicketStatus,
                TicketType = p.TicketType,
                Subject = p.Subject
            }).ToList();
            return Ok(responseDtoList);
        }
        [Authorize("Customer")]
        [HttpPost("CreateTicket")]
        public async Task<IActionResult> Create(int LicenseId,TicketCreateDto request) 
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            TicketValidator tv = new TicketValidator();
            var validationResult = tv.Validate(new Ticket
            {
                Description = request.Description,
                TicketStatus = request.TicketStatus,
                TicketType = request.TicketType,
                CreatedAt = request.CreatedAt,
                UserId = userId
            });
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _ticketService.Create(userId,LicenseId, request);
            return Ok(response);
        }
    }
}

