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
        Task<IEnumerable<Book>> GetAllWithAuthorsCategoriesPublisher();
        Task Insert(Book entity);
        Task Update(Book entity);
        Task Delete(Book entity);
        Task Delete(int id);
    }
}
