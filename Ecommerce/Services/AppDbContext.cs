using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        public DbSet<Admin> tbl_admin {  get; set; }
        public DbSet<Customer> tbl_Customer {  get; set; }

        public DbSet<Category> tbl_Category { get; set; }
        public DbSet<Product> tbl_Product { get; set; }

        public DbSet<Cart> tbl_Cart { get; set; }
        public DbSet <Order> tbl_Order { get; set; }
        public DbSet<Feedback> tbl_Feedback {  get; set; }
        public DbSet<FAQ> tbl_FAQ { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.
                Entity<Product>().HasOne(p=>p.category).
                WithMany(c=>c.products).
                HasForeignKey(p=>p.CategoryId);
        }
    }
}
