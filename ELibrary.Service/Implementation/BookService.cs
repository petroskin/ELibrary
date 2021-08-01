using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using ELibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Service.Implementation
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public void Delete(Book entity)
        {
            _bookRepository.Delete(entity);
        }

        public void Delete(Guid? id)
        {
            _bookRepository.Delete(id);
        }

        public Book Get(Guid? id)
        {
            return _bookRepository.Get(id);
        }

        public IEnumerable<Book> GetAll()
        {
            return _bookRepository.GetAll();
        }

        public void Insert(Book entity)
        {
            _bookRepository.Insert(entity);
        }

        public void Update(Book entity)
        {
            _bookRepository.Update(entity);
        }
    }
}
