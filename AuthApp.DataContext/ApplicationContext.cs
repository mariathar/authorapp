using AuthApp.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AuthApp.DataContext
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        public ApplicationContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {    }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var userFirstId = Guid.NewGuid();
            var userSecondtId = Guid.NewGuid();
            modelBuilder.Entity<User>().HasData(
                new User { Id = userFirstId, Login = "admin@mail.ru", Password = "12345", Role = "admin" },
                new User { Id = userSecondtId, Login = "user@mail.ru", Password = "67890", Role = "user" });

            var authorFirstId = Guid.NewGuid();
            var authorSecondtId = Guid.NewGuid();
            modelBuilder.Entity<Author>().HasData(
              new Author { Id = authorFirstId, FirstName = "Иван", LastName = "Иванов", UserId = userFirstId, DateAdded = DateTime.Now },
              new Author { Id = authorSecondtId, FirstName = "Сергей", LastName = "Сергеев", UserId = userSecondtId, DateAdded = DateTime.Now });

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = Guid.NewGuid(), AuthorId = authorFirstId, Name = "Book1", UserId = userFirstId, DateAdded = DateTime.Now, DateCreate = DateTime.Now }
                );

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

    }
}
