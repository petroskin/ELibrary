using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Service.Interface
{
    public interface IBookService
    {
        IEnumerable<Book> GetAll();
        Book Get(Guid? id);
        void Insert(Book entity);
        void Update(Book entity);
        void Delete(Book entity);
        void Delete(Guid? id);
    }
}
