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
            try
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

               // if (page < 1) page = 1;

                // apply pagination
                return query.Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
            }
            catch (Exception ex)
            {
                // Log error (better: inject ILogger<T> instead of Console)
                Console.WriteLine($"Repository Error in GetAll: {ex.Message}");
                //return Enumerable.Empty<T>();
                //return Enumerable.Empty<T>();
                throw; // rethrow so service layer knows
            }
        }

        public virtual T? GetById(int id)
        {
            try
            {
                return _dbSet.Find(id);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error in GetById for entity {typeof(T).Name}, Id={id}: {ex.Message}");
                throw;
            }
            
        }

        public virtual void Add(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Add for {typeof(T).Name}: {ex.Message}");
                throw; 
            }
        }

        public virtual void Update(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update for {typeof(T).Name}: {ex.Message}");
                throw;
            }
        }

        public virtual void Delete(int id)
        {
            try
            {
                var entity = _dbSet.Find(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete for {typeof(T).Name}, Id={id}: {ex.Message}");
                throw;
            }
        }


        public virtual int GetTotalCount(Expression<Func<T, bool>>? predicate = null)
        {
            try
            {
                return predicate == null ? _dbSet.Count() : _dbSet.Count(predicate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTotalCount for {typeof(T).Name}: {ex.Message}");
                throw;
            }
        }

    }
}
