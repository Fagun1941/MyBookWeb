//using BukyBookWeb.Models;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;

//namespace BukyBookWeb.Data
//{
//    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
//    {
//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
//        {
//        }
//        public DbSet<Category> Categories { get; set; }
//        public DbSet<Product> Products { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            modelBuilder.Entity<Product>()
//                .HasOne(p => p.Category)        
//                .WithMany(c => c.Products)      
//                .HasForeignKey(p => p.CategoryId);
//        }

//    }
//}


using BukyBookWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BukyBookWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Dummy categories

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Books", DisplayOrder = "1", CreateDateTime = DateTime.Now },
                new Category { Id = 2, Name = "Electronics", DisplayOrder = "2", CreateDateTime = DateTime.Now },
                new Category { Id = 3, Name = "Clothing", DisplayOrder = "3", CreateDateTime = DateTime.Now }
            );

            // Dummy products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Title = "C# Basics", Price = 15, CategoryId = 1, Author = "abc", Description = "abdf dkf ddfkdn dfdkfn " },
                new Product { Id = 2, Title = "Laptop", Price = 850, CategoryId = 2, Author = "abc", Description = "abdf dkf ddfkdn dfdkfn " },
                new Product { Id = 3, Title = "T-Shirt", Price = 12, CategoryId = 3, Author = "abc", Description = "abdf dkf ddfkdn dfdkfn " }
            );
        }
    }
}
