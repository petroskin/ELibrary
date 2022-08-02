using ELibrary.Domain.Identity;
using ELibrary.Domain.Identity.Unused;
using ELibrary.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ELibraryUser, ELibraryRole, int, UserClaim, CurrentUserRole, UserLogin, RoleClaim, UserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Publisher> Publisher { get; set; }
        public virtual DbSet<BookAuthor> BookAuthors { get; set; }
        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<CategoriesInBook> BookCategories { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<ELibraryUser> ELibraryUsers { get; set; }
        public virtual DbSet<ELibraryRole> ELibraryRoles { get; set; }
        public virtual DbSet<UserRole> AllUserRoles { get; set; }
        public virtual DbSet<CurrentUserRole> CurrentUserRoles { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<BooksInCart> BooksInCarts { get; set; }
        public virtual DbSet<Rent> Rents { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=<username here>;Password=<password here>;SearchPath=elibrary");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("elibrary");
            base.OnModelCreating(builder);

            // new

            builder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId);

            builder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(ba => ba.AuthorId);

            builder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.Authors)
                .HasForeignKey(ba => ba.BookId);

            builder.Entity<CategoriesInBook>()
                .HasOne(cb => cb.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(cb => cb.CategoryId);

            builder.Entity<CategoriesInBook>()
                .HasOne(cb => cb.Book)
                .WithMany(b => b.Categories)
                .HasForeignKey(cb => cb.BookId);

            builder.Entity<BooksInCart>()
                .HasOne(i => i.Cart)
                .WithMany(i => i.BooksInCart)
                .HasForeignKey(i => i.CartId);

            builder.Entity<BooksInCart>()
                .HasOne(i => i.Book)
                .WithMany()
                .HasForeignKey(i => i.BookId);

            builder.Entity<Rent>()
                .HasOne(i => i.Book)
                .WithMany()
                .HasForeignKey(i => i.BookId);

            builder.Entity<Rent>()
                .HasOne(i => i.User)
                .WithMany(i => i.Rents)
                .HasForeignKey(i => i.UserId);

            builder.Entity<Review>()
                .HasOne(i => i.Book)
                .WithMany(i => i.Reviews)
                .HasForeignKey(i => i.BookId);

            builder.Entity<Review>()
                .HasOne(i => i.User)
                .WithMany(i => i.Reviews)
                .HasForeignKey(i => i.UserId);

            builder.Entity<UserRole>()
                .HasOne(i => i.User)
                .WithMany(i => i.Roles)
                .HasForeignKey(i => i.UserId);

            builder.Entity<UserRole>()
                .HasOne(i => i.Role)
                .WithMany(i => i.Users)
                .HasForeignKey(i => i.RoleId);

            builder.Entity<ELibraryUser>()
                .ToTable(name: "elibuser");

            builder.Entity<ELibraryRole>()
                .ToTable(name: "elibrole");

            builder.Entity<CurrentUserRole>()
                .ToTable(name: "currentuserrole");

            builder.Entity<RoleClaim>()
                .ToTable(name: "roleclaim");

            builder.Entity<UserClaim>()
                .ToTable(name: "userclaim");

            builder.Entity<UserLogin>()
                .ToTable(name: "userlogin");

            builder.Entity<UserToken>()
                .ToTable(name: "usertoken");
        }
    }
}
