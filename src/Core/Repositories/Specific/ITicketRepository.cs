using Models.DTOs;
using Models.Entities;

namespace Core.Repositories.Specific
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        public PagedList<Ticket> GetTickets(PagedParameters ticketsParameters);
        Ticket GetTicketId(Ticket ticketId);
    }
}
