using BukyBookWeb.Models;
namespace BukyBookWeb.IService
{
    public interface IAuditRepository
    {
        Task AddAsync(AuditLog auditLog);
    }
}