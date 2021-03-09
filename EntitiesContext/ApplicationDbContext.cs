using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EntitiesContext
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.FirstName)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.LastName)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.BirthDate)
                .IsRequired();
            modelBuilder.Entity<User>()
                .HasMany(u => u.Books)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Book>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<Book>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Book>()
                .Property(b => b.Name)
                .IsRequired();
            modelBuilder.Entity<Book>()
                .Property(b => b.PublicationDate)
                .IsRequired();

            modelBuilder.Entity<Categorie>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Categorie>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Categories)
                .WithMany(c => c.Books);

            base.OnModelCreating(modelBuilder);
        }
    }
}
