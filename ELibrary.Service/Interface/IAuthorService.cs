using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Service.Interface
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAll();
        Task<Author> Get(int id);
        Task<Author> GetWithBooks(int id);
        Task Insert(Author entity);
        Task Update(Author entity);
        Task Delete(Author entity);
        Task Delete(int id);
    }
}
