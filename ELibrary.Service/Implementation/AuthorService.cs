using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using ELibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Service.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public void Delete(Author entity)
        {
            _authorRepository.Delete(entity);
        }

        public void Delete(Guid? id)
        {
            _authorRepository.Delete(id);
        }

        public Author Get(Guid? id)
        {
            return _authorRepository.Get(id);
        }

        public IEnumerable<Author> GetAll()
        {
            return _authorRepository.GetAll();
        }

        public void Insert(Author entity)
        {
            _authorRepository.Insert(entity);
        }

        public void Update(Author entity)
        {
            _authorRepository.Update(entity);
        }
    }
}
