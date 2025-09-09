using BukyBookWeb.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BukyBookWeb.Repositories
{
    public class BaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll(
            string? search = null,
            int page = 1,
            int pageSize = 10,
            Expression<Func<T, bool>>? searchPredicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            // apply includes
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // apply search filter
            if (!string.IsNullOrEmpty(search) && searchPredicate != null)
            {
                query = query.Where(searchPredicate);
            }

            // apply ordering
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // apply pagination
            return query.Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        public virtual T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }

        public virtual int GetTotalCount(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate == null ? _dbSet.Count() : _dbSet.Count(predicate);
        }
    }
}
