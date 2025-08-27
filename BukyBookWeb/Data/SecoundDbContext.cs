using BukyBookWeb.Models;
using Microsoft.EntityFrameworkCore;

public class SecondDbContext : DbContext
{
    public SecondDbContext(DbContextOptions<SecondDbContext> options)
        : base(options)
    {
    }
    public DbSet<MyTable> Mytables  { get; set; }
}
