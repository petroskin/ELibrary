using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using ELibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Service.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookAuthorRepository _bookAuthorRepository;
        private readonly IBookRepository _bookRepository;
        public AuthorService(IAuthorRepository authorRepository, IBookAuthorRepository bookAuthorRepository, IBookRepository bookRepository)
        {
            _authorRepository = authorRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _bookRepository = bookRepository;
        }
        public async Task Delete(Author entity)
        {
            await _authorRepository.Delete(entity);
        }

        public async Task Delete(int id)
        {
            await _authorRepository.Delete(id);
        }

        public async Task<Author> Get(int id)
        {
            return await _authorRepository.Get(id);
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            return await _authorRepository.GetAll();
        }

        public async Task<Author> GetWithBooks(int id)
        {
            Author author = await _authorRepository.Get(id);
            author.Books = await _bookAuthorRepository.GetByAuthorId(id);
            foreach (BookAuthor bookAuthor in author.Books)
            {
                bookAuthor.Book = await _bookRepository.Get(bookAuthor.BookId);
            }
            return author;
        }

        public async Task Insert(Author entity)
        {
            if (entity.Id != 0)
                throw new Exception("Cannot insert entity with an id!");
            await _authorRepository.Insert(entity);
        }

        public async Task Update(Author entity)
        {
            await _authorRepository.Update(entity);
        }
    }
}
