using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Core.Repositories
{

	public interface IRepository<T> where T : class, new()
	{
		IQueryable<T> GetAll(Expression<Func<T, bool>> expression = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity);
        T GetSingle(Expression<Func<T, bool>> expression = null, bool throwException = true);
		T Add(T entity);
		T Edit(T entity, Action<EntityEntry<T>> rules = null);
		void Remove(T entity);
		void Save();
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    }
}

