using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Service.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category> Get(int id);
        Task<Category> GetWithBooks(int id);
        Task Insert(Category entity);
        Task Update(Category entity);
        Task Delete(Category entity);
        Task Delete(int id);
    }
}
