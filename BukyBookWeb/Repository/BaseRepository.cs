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

        public virtual async Task<IEnumerable<T>> GetAllAsync(
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

                foreach (var include in includes)
                    query = query.Include(include);

                if (!string.IsNullOrEmpty(search) && searchPredicate != null)
                    query = query.Where(searchPredicate);

                if (orderBy != null)
                    query = orderBy(query);

                if (page < 1) throw new Exception("Don't use zero or negative page number");

                return await query.Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Repository Error in GetAllAsync: {ex.Message}");
                throw;
            }
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync for {typeof(T).Name}, Id={id}: {ex.Message}");
                throw;
            }
        }

        public virtual async Task AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddAsync for {typeof(T).Name}: {ex.Message}");
                throw;
            }
        }

        public virtual async Task UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAsync for {typeof(T).Name}: {ex.Message}");
                throw;
            }
        }

        public virtual async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync for {typeof(T).Name}, Id={id}: {ex.Message}");
                throw;
            }
        }

        public virtual async Task<int> GetTotalCountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            try
            {
                return predicate == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(predicate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTotalCountAsync for {typeof(T).Name}: {ex.Message}");
                throw;
            }
        }
    }
}
