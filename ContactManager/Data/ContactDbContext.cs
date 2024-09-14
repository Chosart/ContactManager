using ContactManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Data
{
    public class ContactDbContext : DbContext
    {
        public ContactDbContext(DbContextOptions<ContactDbContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Contact>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Contact>()
                .Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Category)
                .WithMany(cat => cat.Contacts)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Ensure collections are initialized
            modelBuilder.Entity<Category>()
                .HasData(new Category { Id = 1, Name = "Default Category" }); // Example seed data
        }
    }
}
