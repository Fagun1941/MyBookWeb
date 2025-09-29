using BukyBookWeb.Models;

namespace BukyBookWeb.IService
{
    public interface IAuditService
    {
        Task AddAsync(AuditLog auditLog);
    }
}
