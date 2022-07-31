using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Repository.Interface
{
    public interface IBookAuthorRepository : IRepository<BookAuthor>
    {
        Task<IEnumerable<BookAuthor>> GetByAuthorId(int id);
        Task<IEnumerable<BookAuthor>> GetByBookId(int id);
    }
}
