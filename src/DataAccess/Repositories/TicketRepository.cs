using Core.Repositories;
using Core.Repositories.Specific;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entities;

namespace DataAccessLayer.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
	{
		private readonly DbContext _dbContext;

		public TicketRepository(DbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

        public Ticket GetTicketId(Ticket tickettId)
        {
            return FindByCondition(p => p.Id.Equals(tickettId))
                .DefaultIfEmpty(new Ticket())
                .FirstOrDefault();
        }

        public PagedList<Ticket> GetTickets(PagedParameters productParameters)
        {
            return PagedList<Ticket>.ToPagedList(FindAll(),
            productParameters.PageNumber,
                 productParameters.PageSize);
        }
    }
}
