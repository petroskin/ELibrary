using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELibrary.Repository.Implementation
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Book> _entities;
        string errorMessage = string.Empty;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<Book>();
        }
        public void Delete(Book entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Remove(entity);
            _context.SaveChanges();
        }

        public void Delete(Guid? id)
        {
            Book entity = Get(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public Book Get(Guid? id)
        {
            return _entities.Include(i => i.Author).Include(i => i.CategoriesInBook).SingleOrDefault(i => i.Id == id);
        }

        public IEnumerable<Book> GetAll()
        {
            return _entities.Include(i => i.Author).Include(i => i.CategoriesInBook).AsEnumerable();
        }

        public void Insert(Book entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Book entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Update(entity);
            _context.SaveChanges();
        }
    }
}
