using ELibrary.Domain.Identity;
using ELibrary.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ELibraryUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<CategoriesInBook> CategoriesInBooks { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<ELibraryUser> ELibraryUsers { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<BooksInCart> BooksInCarts { get; set; }
        public virtual DbSet<Rent> Rents { get; set; }
        public virtual DbSet<BooksInRent> BooksInRents { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("elibrary");
            base.OnModelCreating(builder);

            builder.Entity<Book>()
                .HasOne(i => i.Author)
                .WithMany(i => i.Books)
                .HasForeignKey(i => i.AuthorId);

            builder.Entity<CategoriesInBook>()
                .HasOne(i => i.Book)
                .WithMany(i => i.CategoriesInBook)
                .HasForeignKey(i => i.BookId);

            builder.Entity<BooksInCart>()
                .HasOne(i => i.Cart)
                .WithMany(i => i.BooksInCart)
                .HasForeignKey(i => i.CartId);

            builder.Entity<BooksInCart>()
                .HasOne(i => i.Book)
                .WithMany()
                .HasForeignKey(i => i.BookId);

            builder.Entity<BooksInRent>()
                .HasOne(i => i.Rent)
                .WithMany(i => i.BooksInRent)
                .HasForeignKey(i => i.RentId);

            builder.Entity<BooksInRent>()
                .HasOne(i => i.Book)
                .WithMany()
                .HasForeignKey(i => i.BookId);

            builder.Entity<Rent>()
                .HasOne(i => i.User)
                .WithMany(i => i.Rents)
                .HasForeignKey(i => i.UserId);
        }
    }
}
