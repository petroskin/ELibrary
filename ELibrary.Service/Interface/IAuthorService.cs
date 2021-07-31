using ELibrary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Service.Interface
{
    public interface IAuthorService
    {
        IEnumerable<Author> GetAll();
        Author Get(Guid? id);
        void Insert(Author entity);
        void Update(Author entity);
        void Delete(Author entity);
        void Delete(Guid? id);
    }
}
