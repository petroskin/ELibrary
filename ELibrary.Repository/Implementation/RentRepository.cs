﻿using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELibrary.Repository.Implementation
{
    public class RentRepository : IRentRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Rent> _entities;
        string errorMessage = string.Empty;

        public RentRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<Rent>();
        }
        public void Delete(Rent entity)
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
            Rent entity = Get(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public Rent Get(Guid? id)
        {
            return _entities.Include(i => i.User).Include(i => i.BooksInRent).ThenInclude(i => i.Book).ThenInclude(i => i.Author)
                .Include(i => i.BooksInRent).ThenInclude(i => i.Book).ThenInclude(i => i.CategoriesInBook).SingleOrDefault(i => i.Id == id);
        }

        public IEnumerable<Rent> GetAll()
        {
            return _entities.Include(i => i.User).Include(i => i.BooksInRent).ThenInclude(i => i.Book).ThenInclude(i => i.Author)
                .Include(i => i.BooksInRent).ThenInclude(i => i.Book).ThenInclude(i => i.CategoriesInBook).AsEnumerable();
        }

        public void Insert(Rent entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Rent entity)
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
