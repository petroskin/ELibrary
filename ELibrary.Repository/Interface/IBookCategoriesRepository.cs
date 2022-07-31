using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Interface
{
    public interface IBookCategoriesRepository : IRepository<CategoriesInBook>
    {
        Task<IEnumerable<CategoriesInBook>> GetByCategoryId(int id);
        Task<IEnumerable<CategoriesInBook>> GetByBookId(int id);
    }
}
