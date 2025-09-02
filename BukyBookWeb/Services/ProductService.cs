using BukyBookWeb.Data;
using System.Security.AccessControl;

namespace BukyBookWeb.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService (ApplicationDbContext context)
        {
            _context = context;
        }


    }
}
