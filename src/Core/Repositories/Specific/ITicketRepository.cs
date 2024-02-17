using Core.Repositories;
using Models.DTOs.Tickets.Create;
using Models.Entities;

namespace Core.Repositories.Specific
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        //object Add(TicketCreateDto request);
    }
}
