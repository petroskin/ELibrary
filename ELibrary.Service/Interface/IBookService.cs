using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Service.Interface
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAll();
        Task<Book> Get(int id);
        Task<Book> GetWithAuthorsCategoriesPublisher(int id);
        Task Insert(Book entity);
        Task Update(Book entity);
        Task Delete(Book entity);
        Task Delete(int id);
        Task AddAuthors(Book book, IEnumerable<Author> authors);
        Task RemoveAuthors(IEnumerable<BookAuthor> authors);
        Task AddCategories(Book book, IEnumerable<Category> categories);
        Task RemoveCategories(IEnumerable<CategoriesInBook> categories);
    }
}
