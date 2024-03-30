using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Product.GetAll;
using Models.DTOs;
using Models.DTOs.Tickets.Create;
using Models.DTOs.Tickets.Edit;
using Models.DTOs.Tickets.GetAll;
using Models.DTOs.Tickets.GetById;

namespace Core.Services
{
	public interface ITicketServices
	{
		 List<TicketGetByIDResponseDto> GetById(int id);
		 Task<TicketCreateResponseDto> Create(int id,int LicenseId,TicketCreateDto request);
		 TicketEditStatusResponseDto Edit(TicketEditStatusDto request);
		 List<TicketGetAllResponseDto> GetAll();
         List<TicketGetAllResponseDto> GetTicketsPagingData([FromQuery] PagedParameters prodParam);
		List<string> GetTicketTypes();
    }
}
