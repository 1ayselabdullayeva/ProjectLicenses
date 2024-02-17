using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Core.Repositories.Specific
{
	public interface IUserRepository : IRepository<User>
	{
      
    }
}
