using Core.Repositories;
using Core.Repositories.Specific;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Tickets.Create;
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

        
    }
}
