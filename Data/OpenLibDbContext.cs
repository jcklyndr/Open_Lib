using Microsoft.EntityFrameworkCore;
using OopProject.Models; 

namespace OopProject.Data
{
    public class OpenLibDbContext : DbContext
    {
        public OpenLibDbContext(DbContextOptions<OpenLibDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookCategory>()
                .HasKey(bc => new { bc.BookId, bc.CategoryId });

            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookCategories)
                .HasForeignKey(bc => bc.BookId)
                .OnDelete(DeleteBehavior.Cascade);  // for relationships sa entities. para delete din

            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BookCategories)
                .HasForeignKey(bc => bc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.User)
                .WithMany(u => u.Requests)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Book)
                .WithMany()
                .HasForeignKey(r => r.BookId);
        }
    }
}
