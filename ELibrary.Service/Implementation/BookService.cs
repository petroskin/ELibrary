using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using ELibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Service.Implementation
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookCategoriesRepository _bookCategoriesRepository;
        private readonly IBookAuthorRepository _bookAuthorRepository;
        public BookService(IBookRepository bookRepository, IBookCategoriesRepository bookCategoriesRepository, IBookAuthorRepository bookAuthorRepository)
        {
            _bookRepository = bookRepository;
            _bookCategoriesRepository = bookCategoriesRepository;
            _bookAuthorRepository = bookAuthorRepository;
        }

        public async Task AddAuthors(Book book, IEnumerable<Author> authors)
        {
            foreach(Author author in authors)
            {
                await _bookAuthorRepository.Insert(new BookAuthor(author, book));
            }
        }

        public async Task AddCategories(Book book, IEnumerable<Category> categories)
        {
            foreach (Category category in categories)
            {
                await _bookCategoriesRepository.Insert(new CategoriesInBook(book, category));
            }
        }

        public async Task Delete(Book entity)
        {
            await _bookRepository.Delete(entity);
        }

        public async Task Delete(int id)
        {
            await _bookRepository.Delete(id);
        }

        public async Task<Book> Get(int id)
        {
            return await _bookRepository.Get(id);
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return await _bookRepository.GetAll();
        }

        public async Task<Book> GetWithAuthorsCategoriesPublisher(int id)
        {
            return await _bookRepository.GetWithAuthorsCategoriesPublisher(id);
        }

        public async Task Insert(Book entity)
        {
            await _bookRepository.Insert(entity);
        }

        public async Task RemoveAuthors(IEnumerable<BookAuthor> authors)
        {
            foreach (BookAuthor author in authors)
            {
                await _bookAuthorRepository.Delete(author);
            }
        }

        public async Task RemoveCategories(IEnumerable<CategoriesInBook> categories)
        {
            foreach (CategoriesInBook category in categories)
            {
                await _bookCategoriesRepository.Delete(category);
            }
        }

        public async Task Update(Book entity)
        {
            await _bookRepository.Update(entity);
        }
    }
}
