using Core.Repositories;
using Models.DTOs;
using Models.DTOs.Tickets.Create;
using Models.Entities;

namespace Core.Repositories.Specific
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        public PagedList<Ticket> GetTickets(PagedParameters ticketsParameters);
        Ticket GetTicketId(Ticket ticketId);
    }
}
