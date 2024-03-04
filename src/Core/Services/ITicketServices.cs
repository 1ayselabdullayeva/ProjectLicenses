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
		public TicketGetByIDResponseDto GetById(int id);
		public  Task<TicketCreateResponseDto> Create(int id,TicketCreateDto request);
		public TicketEditStatusResponseDto Edit(TicketEditStatusDto request);
		public List<TicketGetAllResponseDto> GetAll();
        public List<TicketGetAllResponseDto> GetTicketsPagingData([FromQuery] PagedParameters prodParam);
    }
}
