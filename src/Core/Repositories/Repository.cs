using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Core.Repositories
{
    public abstract class Repository<T> : IRepository<T>
        where T : class, new()
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _table;

        protected Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _table = _dbContext.Set<T>();
        }
        protected DbContext _DbContext { get => _dbContext; }
        protected DbSet<T> _Table { get => _table; }
        public T Add(T entity)
        {
            _table.Add(entity);
            return entity;
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default)
        {
            return await _table.AsNoTracking().FirstOrDefaultAsync(filter, cancellationToken);
        }
        public async Task<T> AddAsync(T entity)
        {
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
        }

        public T Edit(T entity, Action<EntityEntry<T>> rules = null)
        {
            var entry = _dbContext.Entry(entity);
            entry.State = EntityState.Modified;
            return entity;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression = null)
        {
            var query = _table.AsQueryable();
            if (expression != null)
                query = query.Where(expression);

            return query;
        }
        public T GetSingle(Expression<Func<T, bool>> expression = null, bool throwException = true)
        {
            var query = _table.AsQueryable();
            if (expression != null)
                query = query.Where(expression);

            var entity = query.FirstOrDefault();
            if (throwException && entity == null)
                //throw new ResourceNotFoundException("/*Resource not found*/");
                throw new BaseException<ResourceNotFoundException>("Resource not found");

            return entity;
        }
        public void Remove(T entity)
        {
            _table.Remove(entity);
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public IQueryable<T> FindAll()
        {
            return _table
                .AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _table
                .Where(expression)
                .AsNoTracking();
        }
    }
}

