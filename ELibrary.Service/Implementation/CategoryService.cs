using ELibrary.Domain.Models;
using ELibrary.Repository.Interface;
using ELibrary.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary.Service.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task Delete(Category entity)
        {
            await _categoryRepository.Delete(entity);
        }

        public async Task Delete(int id)
        {
            await _categoryRepository.Delete(id);
        }

        public async Task<Category> Get(int id)
        {
            return await _categoryRepository.Get(id);
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<Category> GetWithBooks(int id)
        {
            return await _categoryRepository.GetWithBooks(id);
        }

        public async Task Insert(Category entity)
        {
            await _categoryRepository.Insert(entity);
        }

        public async Task Update(Category entity)
        {
            await _categoryRepository.Update(entity);
        }
    }
}
