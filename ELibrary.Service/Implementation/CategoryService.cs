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
        private readonly IBookCategoriesRepository _bookCategoryRepository;
        private readonly IBookRepository _bookRepository;

        public CategoryService(ICategoryRepository categoryRepository, IBookCategoriesRepository bookCategoriesRepository, IBookRepository bookRepository)
        {
            _categoryRepository = categoryRepository;
            _bookCategoryRepository = bookCategoriesRepository;
            _bookRepository = bookRepository;
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
            Category category = await _categoryRepository.Get(id);
            category.Books = await _bookCategoryRepository.GetByCategoryId(id);
            foreach (CategoriesInBook bookCategory in category.Books)
            {
                bookCategory.Book = await _bookRepository.Get(bookCategory.BookId);
                bookCategory.Book.Categories = await _bookCategoryRepository.GetByBookId(bookCategory.BookId);
                foreach (CategoriesInBook categoriesInBook in bookCategory.Book.Categories)
                {
                    categoriesInBook.Category = await _categoryRepository.Get(categoriesInBook.CategoryId);
                }
            }
            return category;
        }

        public async Task Insert(Category entity)
        {
            if (entity.Id != 0)
                throw new Exception("Cannot insert entity with an id!");
            await _categoryRepository.Insert(entity);
        }

        public async Task Update(Category entity)
        {
            await _categoryRepository.Update(entity);
        }
    }
}
