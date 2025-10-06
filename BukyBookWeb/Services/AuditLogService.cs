using BukyBookWeb.IService;
using BukyBookWeb.Models;
using BukyBookWeb.Data; 
using Microsoft.EntityFrameworkCore;

namespace BukyBookWeb.Services
{
    public class AuditService : IAuditService
    {
        private readonly ApplicationDbContext _context;

        public AuditService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AuditLog auditLog)
        {
            if (auditLog == null)
                throw new ArgumentNullException(nameof(auditLog));

            auditLog.TimeAction = DateTime.Now;

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}
