using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELibrary.Repository.Implementation
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Author> _entities;
        string errorMessage = string.Empty;

        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<Author>();
        }
        public void Delete(Author entity)
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
            Author entity = Get(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public Author Get(Guid? id)
        {
            return _entities.Include(i => i.Books).SingleOrDefault(i => i.Id == id);
        }

        public IEnumerable<Author> GetAll()
        {
            return _entities.Include(i => i.Books).AsEnumerable();
        }

        public void Insert(Author entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Author entity)
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
