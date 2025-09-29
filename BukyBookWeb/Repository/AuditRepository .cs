using BukyBookWeb.Data;
using BukyBookWeb.IService;
using BukyBookWeb.Models;

namespace BukyBookWeb.Repository
{
    public class AuditRepository : IAuditRepository
    {
        private readonly ApplicationDbContext _context;
        public AuditRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AuditLog auditLog)
        {
            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}
