using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using ELibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Service.Implementation
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookCategoriesRepository _bookCategoriesRepository;
        private readonly IBookAuthorRepository _bookAuthorRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        public BookService(IBookRepository bookRepository, 
            IBookCategoriesRepository bookCategoriesRepository, 
            IBookAuthorRepository bookAuthorRepository, 
            IPublisherRepository publisherRepository,
            IAuthorRepository authorRepository,
            ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _bookCategoriesRepository = bookCategoriesRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _publisherRepository = publisherRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
        }

        private async Task AddAuthors(int bookId, IEnumerable<int> authorIds)
        {
            foreach(int authorId in authorIds)
            {
                await _bookAuthorRepository.Insert(new BookAuthor(authorId, bookId));
            }
        }

        private async Task AddCategories(int bookId, IEnumerable<int> categoryIds)
        {
            foreach (int categoryId in categoryIds)
            {
                await _bookCategoriesRepository.Insert(new CategoriesInBook(bookId, categoryId));
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
            IEnumerable<Book> books = await _bookRepository.GetAll();
            foreach(Book book in books)
            {
                book.Categories = await _bookCategoriesRepository.GetByBookId(book.Id);
                foreach(CategoriesInBook categoriesInBook in book.Categories)
                {
                    categoriesInBook.Category = await _categoryRepository.Get(categoriesInBook.CategoryId);
                }
            }
            return await _bookRepository.GetAll();
        }

        public async Task<Book> GetWithAuthorsCategoriesPublisher(int id)
        {
            Book book = await _bookRepository.Get(id);
            book.Publisher = await _publisherRepository.Get(book.PublisherId);
            book.Authors = await _bookAuthorRepository.GetByBookId(id);
            foreach(BookAuthor bookAuthor in book.Authors)
            {
                bookAuthor.Author = await _authorRepository.Get(bookAuthor.AuthorId);
            }
            book.Categories = await _bookCategoriesRepository.GetByBookId(id);
            foreach(CategoriesInBook categoriesInBook in book.Categories)
            {
                categoriesInBook.Category = await _categoryRepository.Get(categoriesInBook.CategoryId);
            }
            return book;
        }

        public async Task Insert(Book entity)
        {
            if (entity.Id != 0)
                throw new Exception("Cannot insert entity with an id!");
            await _bookRepository.Insert(entity);
        }

        private async Task RemoveAuthors(IEnumerable<BookAuthor> authors)
        {
            foreach (BookAuthor author in authors)
            {
                await _bookAuthorRepository.Delete(author);
            }
        }

        private async Task RemoveCategories(IEnumerable<CategoriesInBook> categories)
        {
            foreach (CategoriesInBook category in categories)
            {
                await _bookCategoriesRepository.Delete(category);
            }
        }

        public async Task Update(Book entity)
        {
            Book previousEntity = await GetWithAuthorsCategoriesPublisher(entity.Id);
            IEnumerable<int> authorsToAdd = entity.Authors.Where(ba => !previousEntity.Authors.Select(ba => ba.Id).Contains(ba.Id)).Select(ba => ba.AuthorId);
            IEnumerable<int> categoriesToAdd = entity.Categories.Where(bc => !previousEntity.Categories.Select(bc => bc.Id).Contains(bc.Id)).Select(bc => bc.CategoryId);
            await RemoveAuthors(previousEntity.Authors.Where(ba => !entity.Authors.Select(ba => ba.AuthorId).Contains(ba.AuthorId)));
            await RemoveCategories(previousEntity.Categories.Where(bc => !entity.Categories.Select(bc => bc.CategoryId).Contains(bc.CategoryId)));
            await AddAuthors(entity.Id, authorsToAdd);
            await AddCategories(entity.Id, categoriesToAdd);
            await _bookRepository.Update(entity);
        }

        public async Task<IEnumerable<Book>> GetAllWithAuthorsCategoriesPublisher()
        {
            IEnumerable<Book> books = await _bookRepository.GetAll();
            foreach (Book book in books)
            {
                book.Publisher = await _publisherRepository.Get(book.PublisherId);
                book.Authors = await _bookAuthorRepository.GetByBookId(book.Id);
                foreach (BookAuthor bookAuthor in book.Authors)
                {
                    bookAuthor.Author = await _authorRepository.Get(bookAuthor.AuthorId);
                }
                book.Categories = await _bookCategoriesRepository.GetByBookId(book.Id);
                foreach (CategoriesInBook categoriesInBook in book.Categories)
                {
                    categoriesInBook.Category = await _categoryRepository.Get(categoriesInBook.CategoryId);
                }
            }
            return books;
        }
    }
}
