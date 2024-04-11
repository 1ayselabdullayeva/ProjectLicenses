using Application.FluentValidation;
using Core.Common.Utilities;
using Core.Repositories.Specific;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.DTOs.Product.GetAll;
using Models.DTOs.Tickets.Create;
using Models.DTOs.Tickets.Edit;
using Models.Entities;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : Controller
    {
        private readonly ITicketServices _ticketService;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        public TicketController(ITicketServices ticketService, ITicketRepository ticketRepository, IUserRepository userRepository)
        {
            _ticketService = ticketService;
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
        }
        [HttpGet("getbyid")]
        [HasPermission("Ticket_Get_List")]
        public IActionResult GetTicketById()
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var responseDto = _ticketService.GetById(id);
            return Ok(responseDto);
        }

        [HttpPut("edit")]
        [HasPermission("Ticket_Modify_Status")]

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

        [HasPermission("Tickets_Get_List")]
        [HttpGet("getAllTickets")]
        public IActionResult GetAllTickets()
        {
            var response = _ticketService.GetAll();
            return Ok(response);
        }

        [HttpGet("PaginationTicket")]
        [HasPermission("Tickets_Get_List")]

        public ActionResult<List<ProductGetAllResponseDto>> GetProductPagingData([FromQuery] PagedParameters prodParam,string sortBy)
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
            var response = _ticketService.GetTicketsPagingData(prodParam);
            switch (sortBy)
            {
                case "create":
                    response = response.OrderBy(p => p.CreatedAt).ToList();
                    break;
                case "status":
                    response = response.OrderBy(p => p.TicketStatus).ToList();
                    break;
                default:
                    break;
            }


            return Ok(response);
        }
        [HttpPost("CreateTicket")]
        [HasPermission("Ticket_Create")]
        public async Task<IActionResult> Create(TicketCreateDto request) 
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            TicketValidator tv = new TicketValidator();
            var validationResult = tv.Validate(new Ticket
            {
                Description = request.Description,
                Subject = request.Subject,
                TicketType = request.TicketType
            });
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _ticketService.Create(userId, request);
            return Ok(response);
        }

        [HttpGet("types")]
        [HasPermission("Ticket_Get_List_Type")]
        public IActionResult GetTicketTypes()
        {
            var types = _ticketService.GetTicketTypes();
            return Ok(types);
        }
        [HttpGet("status")]
        [HasPermission("Ticket_Get_List_Status")]
        public IActionResult GetTicketStatus()
        {
            var types = _ticketService.GetTicketStatus();
            return Ok(types);
        }
        [HttpDelete("DeleteTicket")]
        [HasPermission("Ticket_Delete")]
        public IActionResult DeleteTicket(int id) 
        { 
            var response = _ticketService.DeleteTicket(id);
            return Ok(response);
        }
    }
}

